using GrblConnector.Grbl;
using GrblConnector.Grbl.PushMessages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace GrblConnector.Tests.Tests
{
    [TestClass]
    public class StatusMessageTests
    {
        [TestMethod]
        [DataRow("Idle", MachineState.Idle, -1)]
        [DataRow("Run", MachineState.Run, -1)]
        [DataRow("Hold", MachineState.Hold, -1)]
        [DataRow("Hold:0", MachineState.Hold, 0)]
        [DataRow("Hold:1", MachineState.Hold, 1)]
        [DataRow("Jog", MachineState.Jog, -1)]
        [DataRow("Alarm", MachineState.Alarm, -1)]
        [DataRow("Door", MachineState.Door, -1)]
        [DataRow("Door:0", MachineState.Door, 0)]
        [DataRow("Door:1", MachineState.Door, 1)]
        [DataRow("Door:2", MachineState.Door, 2)]
        [DataRow("Door:3", MachineState.Door, 3)]
        [DataRow("Check", MachineState.Check, -1)]
        [DataRow("Home", MachineState.Home, -1)]
        [DataRow("Sleep", MachineState.Sleep, -1)]
        public void MachineStateTest(string machineState, MachineState expectedState, int expectedSubState)
        {
            var line = $"<{machineState}|MPos:0.000,0.000,0.000|FS:0.0,0>";
            var msg = GrblMessage.Parse(line);
            Assert.IsNotNull(msg);
            Assert.IsInstanceOfType(msg, typeof(StatusReportMessage));

            var statusMsg = msg as StatusReportMessage;
            Assert.AreEqual(statusMsg.MachineState, expectedState);
            Assert.AreEqual(statusMsg.MachineSubState, expectedSubState);
        }

        [TestMethod]
        [DataRow("MPos:0.000,-10.000,5.000", PositionType.MachinePosition, new float[] { 0.0f, -10.0f, 5.0f, 0.0f })]
        [DataRow("WPos:-2.500,0.000,11.000", PositionType.WorkPosition, new float[] { -2.5f, 0.0f, 11.0f, 0.0f })]
        public void CurrentPositionTest(string position, PositionType expectedPositionType, float[] expectedCoordinates)
        {
            var line = $"<Idle|{position}|FS:0.0,0>";
            var msg = GrblMessage.Parse(line);
            Assert.IsNotNull(msg);
            Assert.IsInstanceOfType(msg, typeof(StatusReportMessage));

            var statusMsg = msg as StatusReportMessage;
            Assert.AreEqual(statusMsg.PositionType, expectedPositionType);

            var testFloatArray = new float[4];
            statusMsg.CurrentPosition.CopyTo(testFloatArray);

            for (int i = 0; i < testFloatArray.Length; i++)
            {
                Assert.AreEqual(testFloatArray[i], expectedCoordinates[i]);
            }
        }

        [TestMethod]
        [DataRow("WCO:0.000,1.551,5.664", new float[] { 0.0f, 1.551f, 5.664f, 0.0f})]
        public void WorkCoordinateOffsetTest(string workCoordinateOffset, float[] expectedOffset)
        {
            var line = $"<Hold|FS:0.0,0|{workCoordinateOffset}>";

            var msg = GrblMessage.Parse(line);
            Assert.IsNotNull(msg);
            Assert.IsInstanceOfType(msg, typeof(StatusReportMessage));

            var statusMsg = msg as StatusReportMessage;

            var testFloatArray = new float[4];
            statusMsg.WorkCoordinateOffset.CopyTo(testFloatArray);

            for (int i = 0; i < testFloatArray.Length; i++)
            {
                Assert.AreEqual(testFloatArray[i], expectedOffset[i]);
            }
        }

        [TestMethod]
        [DataRow("Bf:15,128", 15, 128]
        public void BufferStateTest(string bufferState, int expectedAvailableBlocksInPlanner, int expectedAvailableBytesInRx)
        {
            var line = $"<Hold|FS:0.0,0|{bufferState},WCO:0.000,1.551,5.664>";

            var msg = GrblMessage.Parse(line);
            Assert.IsNotNull(msg);
            Assert.IsInstanceOfType(msg, typeof(StatusReportMessage));

            var statusMsg = msg as StatusReportMessage;

            Assert.IsNotNull(statusMsg.BufferState);

            Assert.AreEqual(statusMsg.BufferState.AvailableBlocksInPlannerBuffer, expectedAvailableBlocksInPlanner);
            Assert.AreEqual(statusMsg.BufferState.AvailableBytesInRxBuffer, expectedAvailableBytesInRx);
        }
    }
}
