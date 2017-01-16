using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public enum MachineState
    {
        Unknown, Idle, Run, Hold, Jog, Alarm, Door, Check, Home, Sleep
    }

    [Flags]
    public enum InputPinState
    {
        X, Y, Z,// XYZ limit pins, respectively
        Probe, // the probe pin.
        Door, Hold, SoftReset, CycleStart, //the door, hold, soft-reset, and cycle-start pins, respectively.
        A // A-axis limit pin.
    }

    [Flags]
    public enum AccessoryState
    {
        SpindleCW, // indicates spindle is enabled in the CW direction. This does not appear with C.
        SpindleCCW, // indicates spindle is enabled in the CCW direction. This does not appear with S.
        FloodCoolant, // indicates flood coolant is enabled.
        MistCoolant, // indicates mist coolant is enabled.
    }

    public enum PositionType
    {
        MachinePosition,
        WorkPosition
    }


    public class StatusReportMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '<', '>' });
            var messageParts = message.Split('|');

            ParseMachineState(messageParts[0]);
            ParseCurrentPosition(messageParts[1]);

            if (messageParts.Length > 2)
            {
                for (int i = 2; i < messageParts.Length; i++)
                {
                    ParseStatusMessagePart(messageParts[i]);
                }
            }
        }

        private void ParseStatusMessagePart(string statusMessagePart)
        {
            var parts = statusMessagePart.Split(':');
            var messageType = parts[0];
            var messageValue = parts[1];
            switch (messageType)
            {
                case "WCO":
                    WorkCoordinateOffset = ParseCoordinates(messageValue);
                    break;
                case "Bf":
                    var bufferParts = messageValue.Split(',');
                    BufferState = new BufferState
                    {
                        AvailableBlocksInPlannerBuffer = int.Parse(bufferParts[0]),
                        AvailableBytesInRxBuffer = int.Parse(bufferParts[1])
                    };
                    break;
                case "Ln":
                    LineNumber = int.Parse(messageValue);
                    break;
                case "F":
                case "FS":
                    var feedSpeedParts = messageValue.Split(',');
                    Feed = float.Parse(feedSpeedParts[0]);
                    if (feedSpeedParts.Length > 1)
                    {
                        SpindleSpeed = int.Parse(feedSpeedParts[1]);
                    }
                    else {
                        SpindleSpeed = -1;
                    }
                    break;
                case "Pn":
                    foreach (var pin in messageValue)
                    {
                        switch (pin)
                        {
                            case 'X':
                                InputPinState |= InputPinState.X;
                                break;
                            case 'Y':
                                InputPinState |= InputPinState.Y;
                                break;
                            case 'Z':
                                InputPinState |= InputPinState.Z;
                                break;
                            case 'P':
                                InputPinState |= InputPinState.Probe;
                                break;
                            case 'D':
                                InputPinState |= InputPinState.Door;
                                break;
                            case 'H':
                                InputPinState |= InputPinState.Hold;
                                break;
                            case 'R':
                                InputPinState |= InputPinState.SoftReset;
                                break;
                            case 'S':
                                InputPinState |= InputPinState.CycleStart;
                                break;
                            case 'A':
                                InputPinState |= InputPinState.A;
                                break;
                        }
                    }
                    break;
                case "Ov":
                    var overrideParts = messageValue.Split(',');
                    OverrideFeedPercent = int.Parse(overrideParts[0]);
                    OverrideRapidsPercent = int.Parse(overrideParts[1]);
                    OverrideSpindleSpeedPercent = int.Parse(overrideParts[2]);
                    break;
                case "A":
                    foreach (var accessory in messageValue)
                    {
                        switch (accessory)
                        {
                            case 'S':
                                AccessoryState |= AccessoryState.SpindleCW;
                                break;
                            case 'C':
                                AccessoryState |= AccessoryState.SpindleCCW;
                                break;
                            case 'F':
                                AccessoryState |= AccessoryState.FloodCoolant;
                                break;
                            case 'M':
                                AccessoryState |= AccessoryState.MistCoolant;
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void ParseMachineState(string machineStateString)
        {
            var parts = machineStateString.Split(':');
            MachineState = (MachineState)Enum.Parse(typeof(MachineState), parts[0]);

            if (MachineState == MachineState.Unknown)
            {
                throw new ArgumentException($"Unknown machine state: {machineStateString}");
            }

            if (parts.Length == 2)
            {
                MachineSubState = int.Parse(parts[1]);
            }
            else
            {
                MachineSubState = -1;
            }
        }

        private void ParseCurrentPosition(string positionString)
        {
            var parts = positionString.Split(':');
            if (parts[0] == "MPos")
            {
                PositionType = PositionType.MachinePosition;
            }
            else if (parts[0] == "WPos")
            {
                PositionType = PositionType.WorkPosition;
            }
            else
            {
                throw new ArgumentException($"Invalid current position: {positionString}");
            }
            var coordinateString = parts[1];
            CurrentPosition = ParseCoordinates(coordinateString);
        }

        private Vector4 ParseCoordinates(string coordinateString)
        {
            var parts = coordinateString.Split(',');

            var x = float.Parse(parts[0]);
            var y = float.Parse(parts[1]);
            var z = float.Parse(parts[2]);
            var a = parts.Length == 4 ? float.Parse(parts[3]) : 0;

            var coordinates = new Vector4(x, y, z, a);
            return coordinates;
        }

        public MachineState MachineState { get; set; }
        public int MachineSubState { get; set; }

        public PositionType PositionType { get; set; }

        public Vector4 CurrentPosition { get; set; }

        public Vector4 WorkCoordinateOffset { get; set; }

        public BufferState BufferState { get; set; }

        public int LineNumber { get; set; }

        public float Feed { get; set; }

        public int SpindleSpeed { get; set; }

        public InputPinState InputPinState { get; set; }

        public int OverrideFeedPercent { get; set; }

        public int OverrideRapidsPercent { get; set; }

        public int OverrideSpindleSpeedPercent { get; set; }

        public AccessoryState AccessoryState { get; set; }
    }
}
