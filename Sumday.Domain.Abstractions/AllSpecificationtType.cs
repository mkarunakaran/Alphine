namespace Sumday.Domain.Abstractions
{
    public sealed class AllSpecificationtType : Enumeration<AllSpecificationtType, int>
    {
        public AllSpecificationtType(int value, string name)
         : base(value, name)
        {
        }

        public static AllSpecificationtType GetAll => new AllSpecificationtType(0, "GetAll");

        public static AllSpecificationtType Search => new AllSpecificationtType(1, "Search");

        public static implicit operator AllSpecificationtType(int value) => FromValue(value);

        public static implicit operator int(AllSpecificationtType allSpecificationtType) => allSpecificationtType.Value;
    }
}
