using System;
using System.Collections.Generic;
using System.Text;

namespace FileParser_Samples_Tests.Models
{
    public class ICPFile
    {
        public string FileName { get; set; }

        public IEnumerable<ICPCell> Rows { get; set; }
        public string[] Lines { get; set; }

    }
}
