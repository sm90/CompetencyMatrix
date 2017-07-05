using System;
using System.Collections.Generic;

namespace CompetencyMatrix.Models
{
    public partial class Databasechangeloglock
    {
        public int Id { get; set; }
        public bool Locked { get; set; }
        public DateTime? Lockgranted { get; set; }
        public string Lockedby { get; set; }
    }
}
