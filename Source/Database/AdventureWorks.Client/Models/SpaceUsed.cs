using System;
using System.Collections.Generic;

namespace AdventureWorks.Client.Models
{
    public partial class SpaceUsed
    {
        public string TableName { get; set; }
        public string TotalRows { get; set; }
        public string Reserved { get; set; }
        public string TableData { get; set; }
        public string IndexSize { get; set; }
        public string Unused { get; set; }
    }
}
