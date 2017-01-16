using CsvHelper.Configuration;

namespace GrblConnector.MessageStrings
{
    public class BuildOption
    {
        public string OptCode { get; set; }
        public string Explanation { get; set; }

        public bool Enabled { get; set; }
    }

    public sealed class BuildOptionMap : CsvClassMap<BuildOption>
    {
        public BuildOptionMap()
        {
            Map(m => m.OptCode).Name("OPT: Code");
            Map(m => m.Explanation).Name("Build-Option Description");
            Map(m => m.Enabled).Name("State").TypeConverterOption.BooleanValues(true, true, "Enabled");
        }
    }
}
