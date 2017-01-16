using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrblConnector.MessageStrings
{
    public class AlarmCode
    {
        public string AlarmCodeAfter11 { get; set; }
        public string AlarmCodeBefore10 { get; set; }
        public string Explanation { get; set; }
    }

    public sealed class AlarmCodeMap : CsvClassMap<AlarmCode>
    {
        public AlarmCodeMap()
        {
            Map(m => m.AlarmCodeAfter11).Name("Alarm Code in v1.1+");
            Map(m => m.AlarmCodeBefore10).Name("Alarm Message in v1.0-");
            Map(m => m.Explanation).Name("Alarm Description");
        }
    }
}
