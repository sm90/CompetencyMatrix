using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CompetencyMatrix.Utility
{
    public static class DbContextExtensions
    {
	    public static T Upsert<T>(this CompetencyMatrix.Models.CompetencyMatrixContext context, T value, bool add) where T:class
	    {
			EntityEntry<T> trackedEntity;

			if (add)
			{
				trackedEntity = context.Add(value);
			}
			else
			{
				trackedEntity = context.Update(value);
			}

			var affected = context.SaveChanges(true);

			return trackedEntity.Entity;

		}
	}
}
