using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.Grbl.PushMessages
{
    public class CompileOptionPrintoutMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            message = message.Trim(new char[] { '[', ']' });
            var buildOptionString = message.Split(':')[1];
            BuildOptionCodes = buildOptionString.ToCharArray();
        }

        public char[] BuildOptionCodes { get; set; }
    }
}
