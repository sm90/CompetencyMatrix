using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;

namespace CompetencyMatrix.ViewModels
{
	public class PositionMatrixListItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Selected { get; set; }

		public static PositionMatrixListItem FromDbModel(PositionMatrix matrix)
		{
			var result = new PositionMatrixListItem
			{
				Id = matrix.Id,
				Name = matrix.Name
			};

			return result;
		}
	}
}
