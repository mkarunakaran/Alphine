using System.Collections.Generic;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public class State : ValueObject
    {
        private const int DefaultAge = 18;
        private static readonly List<string> UgmaStates = new List<string> { "SC", "VT", "VI", "GU" };
        private static readonly Dictionary<string, int> StateAndAdultAges = new Dictionary<string, int>
        {
            {
                "AL", 19
            },
            {
                "AK", DefaultAge
            },
            {
                "AZ", DefaultAge
            },
            {
                "AR", DefaultAge
            },
            {
                "CA", DefaultAge
            },
            {
                "CO", DefaultAge
            },
            {
                "CT", DefaultAge
            },
            {
                "DE", DefaultAge
            },
            {
                "FL", DefaultAge
            },
            {
                "GA", DefaultAge
            },
            {
                "HI", DefaultAge
            },
            {
                "ID", DefaultAge
            },
            {
                "IL", DefaultAge
            },
            {
                "IN", DefaultAge
            },
            {
                "IA", DefaultAge
            },
            {
                "KS", DefaultAge
            },
            {
                "KY", DefaultAge
            },
            {
                "LA", DefaultAge
            },
            {
                "ME", DefaultAge
            },
            {
                "MD", DefaultAge
            },
            {
                "MA", DefaultAge
            },
            {
                "MI", DefaultAge
            },
            {
                "MN", DefaultAge
            },
            {
                "DC", DefaultAge
            },
            {
                "MS", 21
            },
            {
                "MO", DefaultAge
            },
            {
                "MT", DefaultAge
            },
            {
                "NE", 19
            },
            {
                "NV", DefaultAge
            },
            {
                "NH", DefaultAge
            },
            {
                "NJ", DefaultAge
            },
            {
                "NM", DefaultAge
            },
            {
                "NY", DefaultAge
            },
            {
                "NC", DefaultAge
            },
            {
                "ND", DefaultAge
            },
            {
                "OH", DefaultAge
            },
            {
                "OK", DefaultAge
            },
            {
                "OR", DefaultAge
            },
            {
                "PA", DefaultAge
            },
            {
                "RI", DefaultAge
            },
            {
                "SC", DefaultAge
            },
            {
                "SD", DefaultAge
            },
            {
                "TN", DefaultAge
            },
            {
                "TX", DefaultAge
            },
            {
                "UT", DefaultAge
            },
            {
                "VT", DefaultAge
            },
            {
                "VA", DefaultAge
            },
            {
                "WA", DefaultAge
            },
            {
                "WV", DefaultAge
            },
            {
                "WI", DefaultAge
            },
            {
                "WY", DefaultAge
            }
        };

        private static readonly Dictionary<string, int> MilitaryStateCodes = new Dictionary<string, int>
        {
            {
                "AA", DefaultAge
            },
            {
                "AE", DefaultAge
            },
            {
                "AP", DefaultAge
            }
        };

        public State(string text, bool allowMilitaryState = false)
        {
            var isValid = StateAndAdultAges.ContainsKey(text) ||
                   (allowMilitaryState && MilitaryStateCodes.ContainsKey(text));

            if (!isValid)
            {
                throw new InvalidObjectException(nameof(State));
            }

            this.Text = text.ToUpperInvariant();
            this.AgeOfAdult = allowMilitaryState ? MilitaryStateCodes[text] : StateAndAdultAges[text];
            this.AllowMilitaryState = allowMilitaryState;
        }

        public int AgeOfAdult { get; }

        public bool AllowMilitaryState { get; }

        public bool IsUgma => UgmaStates.Contains(this.Text);

        public string Text { get; }

        public static implicit operator State(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return new State(text);
        }

        public static implicit operator string(State stateCode)
        {
            return stateCode?.Text;
        }

        public override string ToString()
        {
            return this.Text;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Text;
        }
    }
}
