using System;
using System.Linq;

namespace Marcus.Common
{
    public static class NullableUtil
    {
        public static bool AllHaveValue<T>(params T?[] args) where T : struct
        {
            return args.All(x => x.HasValue);
        }

        public static bool AllDontHaveValue<T>(params T?[] args) where T : struct
        {
            return args.All(x => !x.HasValue);
        }

        public static bool AllHaveValueAndNotDefault(params Guid?[] args)
        {
            return args.All(x => x.HasValue && x != default(Guid));
        }
    }
}