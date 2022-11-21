using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileParser_Samples_Tests.Models
{
    public class ParsedRow
    {
        public IEnumerable<ParsedCell> Cells { get; }

        public ParsedRow(IEnumerable<ParsedCell> cells)
        {
            Cells = cells.ToList();
        }
    }
}
