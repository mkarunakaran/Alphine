using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Shared.ValueObjects
{
    public sealed class PaperStatementPreference : Enumeration<PaperStatementPreference, string>
    {
        public PaperStatementPreference(string type, string value)
         : base(type, value)
        {
        }

        public static PaperStatementPreference None => new PaperStatementPreference("A", "None");

        public static PaperStatementPreference StatementAndTaxForm => new PaperStatementPreference("H", "StatementAndTaxForm");

        public static PaperStatementPreference TaxForm => new PaperStatementPreference("E", "TaxForm");

        public static implicit operator PaperStatementPreference(string value) => FromName(value) ?? FromValue(value);

        public static implicit operator string(PaperStatementPreference paperStatementPreference) => paperStatementPreference.Value;
    }
}
