using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.EntityFrameworkCore;

namespace CompetencyMatrix.ViewModels
{
	public class SkillLevelCriteriaMatrix
	{
		public int SkillId { get; set; }
		public List<SkillLevelCriteriaMatrixRow> Rows { get; set; } = new List<SkillLevelCriteriaMatrixRow>();
		public List<SkillLevelModelEntry> Levels { get; set; } = new List<SkillLevelModelEntry>();

	    public bool IsEditable { get; set; }

	    protected List<SkillLevelCriteriaMatrixEntry> AllEntries
		{
			get { return Rows.SelectMany(r => r.Entries).ToList(); }
		}

		public SkillLevelCriteriaMatrix()
		{
		}

		public int Save(CompetencyMatrixContext dbContext)
		{
			var dbSkillLevelCriterias = dbContext.SkillLevelCriteria.ToList();

			//Sanitize the input
			Rows.RemoveAll(r => r.SkillCriteria == null);
			Rows.ForEach(r => r.Entries.RemoveAll(e => e.SkillCriteria == null || e.SkillLevelModel == null));

			foreach (var level in Levels)
			{
				var dbSkillLevelModel = dbContext.SkillLevelModel.SingleOrDefault(c => c.Id == level.SkillLevelModelId);
				if (dbSkillLevelModel == null)
				{
					dbSkillLevelModel = new SkillLevelModel();
					dbSkillLevelModel.Name = level.SkillLevelModelName;
					dbSkillLevelModel.Quality = dbContext.SkillLevelModel.Max(e => e.Quality) + 10;
					dbContext.Add(dbSkillLevelModel);
					dbContext.SaveChanges();
				}
				else
				{
					dbSkillLevelModel.Name = level.SkillLevelModelName;
					dbContext.Update(dbSkillLevelModel);
				}
			}

			foreach (var row in Rows)
			{
				var dbSkillCriteria = dbContext.SkillCriteria.SingleOrDefault(c => c.Id == row.SkillCriteria.SkillCriteriaId);
				if (dbSkillCriteria == null)
				{
					dbSkillCriteria = new SkillCriteria();
					dbSkillCriteria.Name = row.SkillCriteria.SkillCriteriaName;
					dbSkillCriteria.Description = row.SkillCriteria.SkillCriteriaDescription;
					dbSkillCriteria.SkillId = SkillId;
					dbContext.Add(dbSkillCriteria);

					dbContext.SaveChanges();

					row.SkillCriteria.SkillCriteriaId = dbSkillCriteria.Id;
					row.Entries.ForEach(e => e.SkillCriteria = row.SkillCriteria);
				}
				else
				{
					dbSkillCriteria.Name = row.SkillCriteria.SkillCriteriaName;
					dbSkillCriteria.Description = row.SkillCriteria.SkillCriteriaDescription;
					dbContext.Update(dbSkillCriteria);
				}

				foreach (var entry in row.Entries)
				{
					if (entry.Checked)
					{
						var dbSkillLevelCriteria =
							dbSkillLevelCriterias.SingleOrDefault(
								slc => slc.SkillCriteriaId == entry.SkillCriteria.SkillCriteriaId && slc.SkillLevelModelId == entry.SkillLevelModel.SkillLevelModelId);

						if (dbSkillLevelCriteria == null)
						{
							dbSkillLevelCriteria = new SkillLevelCriteria();
							dbSkillLevelCriteria.SkillCriteriaId = entry.SkillCriteria.SkillCriteriaId;
							dbSkillLevelCriteria.SkillLevelModelId = entry.SkillLevelModel.SkillLevelModelId;
							dbContext.Add(dbSkillLevelCriteria);
						}
					}
					else
					{
						var dbSkillLevelCriteria =
							dbSkillLevelCriterias.SingleOrDefault(
								slc => slc.SkillCriteriaId == entry.SkillCriteria.SkillCriteriaId && slc.SkillLevelModelId == entry.SkillLevelModel.SkillLevelModelId);

						if (dbSkillLevelCriteria != null)
						{
							dbContext.Remove(dbSkillLevelCriteria);
						}
					}
				}
			}

			//Clean up deleted criterias, that is those that are missing from the model
			var existingDbCriterias = dbContext.SkillCriteria.Where(c => c.SkillId == SkillId).ToList();
			var deletedDbCriterias = existingDbCriterias.Where(c => !Rows.Select(r => r.SkillCriteria.SkillCriteriaId).Contains(c.Id)).ToList();
			var deletedDbLevelCriterias =
				dbContext.SkillLevelCriteria.Where(slc => deletedDbCriterias.Select(c => c.Id).Contains(slc.SkillCriteriaId))
					.ToList();

			dbContext.SkillLevelCriteria.RemoveRange(deletedDbLevelCriterias);
			dbContext.SkillCriteria.RemoveRange(deletedDbCriterias);

			var affected = dbContext.SaveChanges(true);
			return affected;
		}

		public SkillLevelCriteriaMatrix(Skill skill)
		{
			SkillId = skill.Id;
			Rows = new List<SkillLevelCriteriaMatrixRow>();

			var skillCriterias = skill.SkillCriteria.ToList();
			var skillLevelModels = skill.EvaluationModel?.SkillEvaluationModelLevel.Select(eml => eml.SkillLevelModel) ?? Enumerable.Empty<SkillLevelModel>();

			Levels = skillLevelModels?.OrderBy(s => s.Quality).Select(s => new SkillLevelModelEntry { SkillLevelModelId = s.Id, SkillLevelModelName = s.Name }).ToList();


			foreach (var criteria in skillCriterias.OrderBy(c => c.Name))
			{
				var skillCriteriaEntry = new SkillCriteriaEntry
				{
					SkillCriteriaId = criteria.Id,
					SkillCriteriaName = criteria.Name,
					SkillCriteriaDescription = criteria.Description
				};

				var skillLevelCriterias = criteria.SkillLevelCriteria.ToList();

				SkillLevelCriteriaMatrixRow row = CreateRow(skillCriteriaEntry, Levels, skillLevelCriterias);

				Rows.Add(row);
			}
		}

		public void AddCriteria(string criteriaName)
		{
			var skillCriteriaEntry = new SkillCriteriaEntry
			{
				SkillCriteriaName = criteriaName,
			};

			SkillLevelCriteriaMatrixRow row = CreateRow(skillCriteriaEntry, Levels, Enumerable.Empty<SkillLevelCriteria>());

			Rows.Add(row);
		}

		public SkillLevelCriteriaMatrixRow CreateRow(SkillCriteriaEntry skillCriteriaEntry, List<SkillLevelModelEntry> skillLevelModelEntries, IEnumerable<SkillLevelCriteria> dbSkillLevelCriterias)
		{
			var row = new SkillLevelCriteriaMatrixRow();

			row.SkillCriteria = skillCriteriaEntry;

			foreach (var levelModelEntry in skillLevelModelEntries)
			{
				var entry = new SkillLevelCriteriaMatrixEntry();
				entry.SkillLevelModel = levelModelEntry;
				entry.SkillCriteria = skillCriteriaEntry;

				entry.Checked =
					dbSkillLevelCriterias.Any(
						slc => slc.SkillLevelModelId == levelModelEntry.SkillLevelModelId && slc.SkillCriteriaId == skillCriteriaEntry.SkillCriteriaId);

				row.Entries.Add(entry);
			}

			return row;
		}

		public SkillLevelCriteriaMatrix Rebuild(CompetencyMatrixContext dbContext)
		{
			var resultMatrix = new SkillLevelCriteriaMatrix();
			resultMatrix.SkillId = this.SkillId;

			var skill = dbContext.Skill
				.Include(s => s.EvaluationModel).ThenInclude(em => em.SkillEvaluationModelLevel).ThenInclude(eml => eml.SkillLevelModel)
				.Single(s => s.Id == this.SkillId);

			var dbSkillLevelCriterias = dbContext.SkillLevelCriteria
			.Where(slc=>slc.SkillCriteria.SkillId==this.SkillId)
			.ToList();

			var skillLevelModels = skill.EvaluationModel?.SkillEvaluationModelLevel.Select(eml => eml.SkillLevelModel) ?? Enumerable.Empty<SkillLevelModel>();

			resultMatrix.Levels =
				skillLevelModels?.OrderBy(s => s.Quality)
					.Select(s => new SkillLevelModelEntry {SkillLevelModelId = s.Id, SkillLevelModelName = s.Name})
					.ToList() ?? Enumerable.Empty<SkillLevelModelEntry>().ToList();
			resultMatrix.Rows = new List<SkillLevelCriteriaMatrixRow>();

			foreach (var row in this.Rows)
			{
				var resultRow = new SkillLevelCriteriaMatrixRow();
				resultRow.SkillCriteria = row.SkillCriteria;

				//Keep hold of the existing state of the entries
				var existingEntries = row.Entries;
				var skillCriteriaEntry = row.SkillCriteria;
				
				foreach (var levelModelEntry in resultMatrix.Levels)
				{
					var entry = new SkillLevelCriteriaMatrixEntry();
					entry.SkillLevelModel = levelModelEntry;
					entry.SkillCriteria = skillCriteriaEntry;

					var existingEntry =
						existingEntries.SingleOrDefault(
							e =>
								e.SkillLevelModel.SkillLevelModelId == levelModelEntry.SkillLevelModelId &&
								e.SkillCriteria.SkillCriteriaId == skillCriteriaEntry.SkillCriteriaId);
					if (existingEntry != null)
					{
						entry.Checked = existingEntry.Checked;
					}
					else
					{
						entry.Checked =
							dbSkillLevelCriterias.Any(
								slc =>
									slc.SkillLevelModelId == levelModelEntry.SkillLevelModelId &&
									slc.SkillCriteriaId == skillCriteriaEntry.SkillCriteriaId);
					}

					resultRow.Entries.Add(entry);
				}

				resultMatrix.Rows.Add(resultRow);
			}

			return resultMatrix;
		}
	}

	public class SkillLevelModelEntry
	{
		public int SkillLevelModelId { get; set; }
		public string SkillLevelModelName { get; set; }
	}

	public class SkillCriteriaEntry
	{
		public int SkillCriteriaId { get; set; }
		public string SkillCriteriaName { get; set; }
		public string SkillCriteriaDescription { get; set; }
	}

	public class SkillLevelCriteriaMatrixRow
	{
		public SkillCriteriaEntry SkillCriteria { get; set; }

		public List<SkillLevelCriteriaMatrixEntry> Entries { get; set; } = new List<SkillLevelCriteriaMatrixEntry>();
	}

	public class SkillLevelCriteriaMatrixEntry
	{
		public SkillLevelModelEntry SkillLevelModel { get; set; }
		public SkillCriteriaEntry SkillCriteria { get; set; }
		public bool Checked { get; set; }
	}
}
