using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class GcodeParserStateMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            var modalStatesString = message.Split(':')[1];
            GcodeModalStates = modalStatesString.Split(' ');
        }

        public string[] GcodeModalStates { get; set; }
    }
}
