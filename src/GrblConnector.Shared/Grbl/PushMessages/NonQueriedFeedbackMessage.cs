using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class NonQueriedFeedbackMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            Message = message.Split(':')[1];
        }
        public string Message { get; set; }
    }
}
