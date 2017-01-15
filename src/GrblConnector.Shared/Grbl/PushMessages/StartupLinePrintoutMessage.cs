using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class StartupLinePrintoutMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.TrimStart(new char[] { '$', 'N' });
            var messageParts = message.Split(':');
            LineNumber = int.Parse(messageParts[0]);
            Value = messageParts[1];
        }

        public int LineNumber { get; set; }

        public string Value { get; set; }      
    }
}
