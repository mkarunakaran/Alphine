using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common
{
    internal class DeltaUpdateConfiguration
    {
        public bool UseDeltaUpdateStrategy { get; private set; }

        internal void EnableDeltaUpdateStrategy() => this.UseDeltaUpdateStrategy = true;
    }
}
