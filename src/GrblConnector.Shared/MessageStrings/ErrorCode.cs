using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.MessageStrings
{
    public class ErrorCode
    {
        public string ErrorCodeAfter11 { get; set; }
        public string ErrorCodeBefore10 { get; set; }
        public string Explanation { get; set; }
    }

    public sealed class ErrorCodeMap : CsvClassMap<ErrorCode>
    {
        public ErrorCodeMap()
        {
            Map(m => m.ErrorCodeAfter11).Name("Error Code in v1.1+");
            Map(m => m.ErrorCodeBefore10).Name("Error Message in v1.0-");
            Map(m => m.Explanation).Name("Error Description");

        }
    }
}
