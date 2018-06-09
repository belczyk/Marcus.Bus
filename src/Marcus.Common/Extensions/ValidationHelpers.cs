using System.Collections.Generic;
using System.Linq;

namespace Marcus.Common
{
    public static class ValidationHelpers
    {
        public static bool NotDefault<T>(this T value)
        {
            return !value.Equals(default(T));
        }

        public static bool NotNull<T>(this T value)
        {
            return value != null;
        }

        public static bool NotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
    }
}