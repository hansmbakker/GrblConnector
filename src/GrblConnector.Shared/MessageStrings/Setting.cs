using CsvHelper.Configuration;

namespace GrblConnector.MessageStrings
{
    public class GrblSetting
    {
        public string Code { get; set; }

        public string Setting { get; set; }

        public string Unit { get; set; }

        public string Explanation { get; set; }

        public bool Enabled { get; set; }
    }

    public sealed class GrblSettingMap : CsvClassMap<GrblSetting>
    {
        public GrblSettingMap()
        {
            Map(m => m.Code).Name("$-Code");
            Map(m => m.Setting).Name("Setting");
            Map(m => m.Unit).Name("Units");
            Map(m => m.Explanation).Name("Setting Description");
        }
    }
}
