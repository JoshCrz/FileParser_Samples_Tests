using FileParser_Samples_Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileParser_Samples_Tests.Models
{
    public class ParsedCell
    {
        private readonly string value;

        public string Value => value.Trim();

        public ParsedCell(string value) => this.value = value;

        public ParsedCell(IEnumerable<char> chars) => value = new string(chars.ToArray());

        public bool CompareValueWithoutWhitespace(string valueToCompare) => valueToCompare.RemoveWhitespace() == value.RemoveWhitespace();
    }
}
