using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class SettingsPrintoutMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.TrimStart(new char[] { '$' });
            var messageParts = message.Split(':');
            Setting = int.Parse(messageParts[0]);
            Value = messageParts[1];
        }

        public int Setting { get; set; }

        public string Value { get; set; }
    }
}
