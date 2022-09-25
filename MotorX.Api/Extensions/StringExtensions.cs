using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string? str)
        {
            return string.IsNullOrWhiteSpace(str) == false;
        }

        public static bool Like(this string? str, string? other)
        {
            if (str is null || other is null) return false;
            return str.ToLower().Contains(other.ToLower());
        }
    }
}
