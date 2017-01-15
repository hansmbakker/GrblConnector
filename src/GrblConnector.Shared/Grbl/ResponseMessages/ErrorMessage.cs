using System;
using System.Diagnostics;

namespace GrblConnector.Grbl.ResponseMessages
{

    public class ErrorMessage : GrblMessage
    {
        protected override void OnLoadMessage(string message)
        {
            ErrorCode = int.Parse(message.Split(':')[1]);
        }

        public int ErrorCode { get; set; }

        public ErrorMessageType ErrorType
        {
            get
            {
                //TODO: use csv at https://github.com/gnea/grbl/tree/master/doc/csv
                return (ErrorMessageType)ErrorCode;
            }
        }

        public override string ToString()
        {
            if (ErrorType == ErrorMessageType.Unknown)
            {
                var errorString = $"Unknown error type! Error code {ErrorCode}";
                Debug.WriteLine(errorString);
                return errorString;
            }

            return Enum.GetName(typeof(ErrorMessageType), ErrorType);
        }
    }
}
