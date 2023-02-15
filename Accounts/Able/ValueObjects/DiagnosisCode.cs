using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.ShareHolder.Accounts.Able.ValueObjects
{
    public class DiagnosisCode : Enumeration<DiagnosisCode, int>
    {
        public DiagnosisCode(int value, string name)
         : base(value, name)
        {
        }

        public static DiagnosisCode DevelopmentalDisorder => new DiagnosisCode(1, nameof(DevelopmentalDisorder));

        public static DiagnosisCode IntellectualDisability => new DiagnosisCode(2, nameof(IntellectualDisability));

        public static DiagnosisCode PsychiatricDisorder => new DiagnosisCode(3, nameof(PsychiatricDisorder));

        public static DiagnosisCode NervousDisorder => new DiagnosisCode(4, nameof(NervousDisorder));

        public static DiagnosisCode CongenitalAnomalies => new DiagnosisCode(5, nameof(CongenitalAnomalies));

        public static DiagnosisCode RespiratoryDisorder => new DiagnosisCode(6, nameof(RespiratoryDisorder));

        public static DiagnosisCode Other => new DiagnosisCode(7, nameof(Other));

        public static implicit operator DiagnosisCode(int type) => FromValue(type);

        public static implicit operator DiagnosisCode(string name) => FromName(name);

        public static implicit operator int(DiagnosisCode diagnosisCode) => diagnosisCode.Value;
    }
}
