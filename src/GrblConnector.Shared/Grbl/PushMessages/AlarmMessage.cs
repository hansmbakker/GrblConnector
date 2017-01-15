using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public enum AlarmType
    {
        Unknown = 0,

        ///<summary>
        ///Hard limit triggered. Machine position is likely lost due to sudden and immediate halt. Re-homing is highly recommended.
        ///</summary>
        HardLimitTriggered = 1,

        ///<summary>
        ///G-code motion target exceeds machine travel. Machine position safely retained. Alarm may be unlocked.
        ///</summary>
        GcodeTargetTooFar = 2,

        ///<summary>Reset while in motion. Grbl cannot guarantee position. Lost steps are likely. Re-homing is highly recommended.
        ///</summary>
        ResetWhileInMotion = 3,

        ///<summary>Probe fail. The probe is not in the expected initial state before starting probe cycle, where G38.2 and G38.3 is not triggered and G38.4 and G38.5 is triggered.
        ///</summary>
        ProbeFailUnexpectedInitialState = 4,

        ///<summary>Probe fail. Probe did not contact the workpiece within the programmed travel for G38.2 and G38.4.
        ///</summary>
        ProbeFailDidNotContactWithinProgrammedTravel = 5,

        ///<summary>Homing fail. Reset during active homing cycle.
        ///</summary>
        HomingFailReset = 6,

        ///<summary>Homing fail. Safety door was opened during active homing cycle.
        ///</summary>
        HomingFailSafetyDoorOpened = 7,

        ///<summary>Homing fail. Cycle failed to clear limit switch when pulling off. Try increasing pull-off setting or check wiring.
        ///</summary>
        HomingFailLimitSwitchNotCleared = 8,

        ///<summary>
        ///Homing fail. Could not find limit switch within search distance. Defined as 1.5 * max_travel on search and 5 * pulloff on locate phases.
        ///</summary>
        HomingFailLimitSwitchNotFound = 9,
    }

    public class AlarmMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            AlarmCode = int.Parse(message.Split(':')[1]);

        }

        public int AlarmCode { get; set; }

        public AlarmType AlarmType
        {
            get
            {
                return (AlarmType)AlarmCode;
            }
        }
    }
}
