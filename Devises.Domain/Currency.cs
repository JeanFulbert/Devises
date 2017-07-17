namespace Devises.Domain
{
    using System;

    public class Currency : ValueObject<Currency>
    {
        private readonly string name;

        public Currency(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
        }

        protected override bool EqualsCore(Currency other) =>
            this.name == other.name;

        protected override int GetHashCodeCore() =>
            GetHashCodeCombiner.Combine(this.name);

        public override string ToString() =>
            this.name;
    }
}
