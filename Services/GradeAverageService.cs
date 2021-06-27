using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolGradebook.Models.GradeAverage;

namespace SchoolGradebook.Services
{

	public class GradeAverageService
	{

		private readonly SchoolContext context;

		public GradeAverageService(SchoolContext context)
		{
			this.context = context;
		}

		public async Task<GradeAverage> GetGradeAverageForSubjectInstance(int subjectInstanceId)
		{


			GradeAverage gradeAverage = await context.GradeAverages
					.Where(s => s.SubjectInstanceId == subjectInstanceId && s.StudentId == -1)					
					.OrderByDescending(p => p.Added)
					.AsNoTracking()
					.FirstOrDefaultAsync();
			return gradeAverage;

		}
		/// <summary>
		/// Adds a grade average for subject instance
		/// </summary>
		/// <param name="gradeAverage"></param>
		/// <returns></returns>
		public async Task AddGradeAverageForSubjectInstace(GradeAverage gradeAverage)
		{


			await context.GradeAverages.AddAsync(gradeAverage);
			await context.SaveChangesAsync();
			//no necessary checks as this is internal calculation happening on previously verified data
		}

		public async Task<GradeAverage> GetGradeAverageForStudent(int subjectInstanceId, int studentId)
		{

			GradeAverage gradeAverage = await context.GradeAverages
				.Where(s => s.SubjectInstanceId == subjectInstanceId && s.TeacherId == -1)
				.OrderByDescending(p => p.Added)
				.AsNoTracking()
				.FirstOrDefaultAsync();
			return gradeAverage;






		}


	}
}