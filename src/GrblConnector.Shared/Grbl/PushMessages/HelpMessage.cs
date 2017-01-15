using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class HelpMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            var availableCommandsString = message.Split(':')[1];
            AvailableGrblCommands = availableCommandsString.Split(' ');
        }

        public string[] AvailableGrblCommands { get; set; }
    }
}
