using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class ToolLengthOffsetMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            var offsetString = message.Split(':')[1];

            Offset = float.Parse(offsetString);
        }

        public float Offset { get; set; }


    }
}
