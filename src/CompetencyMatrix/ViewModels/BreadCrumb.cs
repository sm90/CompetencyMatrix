using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class BreadCrumb
    {
		public int Id { get; set; }
	    public string Path { get; set; }
		public bool IsCategory { get; set; }
    }
}
