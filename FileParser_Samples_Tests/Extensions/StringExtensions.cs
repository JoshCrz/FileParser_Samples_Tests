using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileParser_Samples_Tests.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str == string.Empty;
        }

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
