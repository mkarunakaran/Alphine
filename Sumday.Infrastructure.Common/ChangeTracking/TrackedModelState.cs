﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Infrastructure.Common.ChangeTracking
{
    internal enum TrackedModelState
    {
        New,
        Removed,
        Existing
    }
}
