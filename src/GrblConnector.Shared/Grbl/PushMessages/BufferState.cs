namespace GrblConnector.Grbl.PushMessages
{
    public class BufferState
    {
        public int AvailableBlocksInPlannerBuffer { get; set; }

        public int AvailableBytesInRxBuffer { get; set; }
    }
}