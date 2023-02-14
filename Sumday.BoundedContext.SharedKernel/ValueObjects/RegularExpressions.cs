namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public static class RegularExpressions
    {
        public const string Alpha = "^([A-Za-z]*)$";

        public const string AlphaAndDash = "^([A-Za-z-]*)$";

        public const string AlphaAndSpace = @"^([A-Za-z ]*)$";

        public const string AlphaDashAndSpace = @"^([A-Za-z \-]*)$";

        public const string AlphaDashAndSpaceApos = @"^([A-Za-z \-'\.]*)$";

        public const string AlphaNumbericDashAndSpaceAposCommaPeriodAmp = @"^([A-Za-z0-9 \-'\.&,()]*)$";

        public const string AlphaNumbericDashAndSpaceAposCommaPeriodAndParenthesis = @"^([A-Za-z0-9 \-'\.&,()]*)$";

        public const string Email = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

        public const string MultipleSpaces = @"(\s{2,})";

        public const string Numeric = @"^([\d]+)$";

        public const string Phone = @"^\d{3}-?\d{3}\-?\d{4}$";

        public const string PoBox = @"(?i)\b(?:p\.?\s*o\.?|post\s+office)\s+box\b";

        public const string PostalCode = @"^(\d{5})-?(\d{4})?$";

        public const string FormatSsn = @"^([\d]{3})-?([\d]{2})-?([\d]{4})$";

        public const string Ein = @"^([\d]{2})-?([\d]{7})$";

        public const string AllowedCharacters = @"^([A-Za-z0-9 \-'\.&@#,()_:`\/]*)$";

        public const string Ssn = @"^(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0{4})\d{4}$";

        public static readonly string UgmaUtma = @"U[T|G]MA[ |\\]\w\w";
    }
}
