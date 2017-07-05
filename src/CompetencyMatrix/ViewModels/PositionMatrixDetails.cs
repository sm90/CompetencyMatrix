using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace CompetencyMatrix.ViewModels
{
	public class PositionMatrixDetails
	{
        [HiddenInput]
		public int Id { get; set; }
		[DisplayName("Matrix Name")]
        [Required]

        public string Name { get; set; }

		public string Description { get; set; }

		public List<PositionMatrixDetails> Parents { get; set; } = new List<PositionMatrixDetails>();
		public List<PositionMatrixDetails> Children { get; set; } = new List<PositionMatrixDetails>();

        public bool HasChildren => Children.Count>0;

	    public string ChildrenPlainList => string.Join(", ", Children.Select(x=>x.Name));

	    public bool IsPublic { get; set; }

        public string Owner { get; set; }

		public static PositionMatrixDetails FromDbModel(PositionMatrix matrix, bool recursive=true )
		{
		    var result = new PositionMatrixDetails
		    {
		        Id = matrix.Id,
		        Name = matrix.Name,
		        Description = matrix.Description,
		        IsPublic = matrix.IsPublic,
		        Owner = matrix.Owner.UserName
		    };

		    if(recursive)
			{
				result.Children = matrix.AllChildren
				.Select(e => FromDbModel(e, false))
				.ToList();
			}

			if (recursive)
			{
				result.Parents = matrix.AllParents
				.Select(e => FromDbModel(e, false))
				.ToList();
			}


			return result;
		}
	}
}
