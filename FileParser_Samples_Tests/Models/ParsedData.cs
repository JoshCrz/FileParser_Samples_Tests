using FileParser_Samples_Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileParser_Samples_Tests.Models
{
    public class ParsedData
    {
        public IEnumerable<ParsedRow> Rows { get; }
        public IEnumerable<ParsedRow> Headers { get; }

        public ParsedData(IEnumerable<ParsedRow> parsedRows, IEnumerable<ParsedRow> headers = null)
        {
            Rows = parsedRows.Where(r => !r.Cells.All(c => c.Value.IsNullOrEmpty())).ToList();
            Headers = (headers ?? Enumerable.Empty<ParsedRow>()).ToList();
        }
    }
}
