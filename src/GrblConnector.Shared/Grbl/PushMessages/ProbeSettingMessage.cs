using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class ProbeSettingMessage : CoordinateSettingMessage
    {
        protected override void OnLoadMessage(string message)
        {
            base.OnLoadMessage(message);
            message = message.Trim(new char[] { '[', ']' });
            char probeSuccesfulChar = message[message.Length - 1];
            LastProbeCycleWasSuccesful = probeSuccesfulChar == '1';
        }

        public bool LastProbeCycleWasSuccesful { get; set; }
    }
}
