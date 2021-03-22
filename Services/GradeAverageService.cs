using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolGradebook.Models.GradeAverage;

namespace SchoolGradebook.Services

	public class GradeAverageService {

	private readonly SchoolContext context;

	public GradeAverageService(SchoolContext context)
	{
		this.context = context;
	}

		public async Task<GradeAverage> getGradeAverageForSubjectInstance(int subjectInstanceId) {


			GradeAverage gradeAverage = await context.GradeAverages
					.Where(s => s.SubjectInstanceId == subjectInstanceId)
					.Include(g => g.Id)
					.Include(g => g.SubjectInstanceId)
					.Include(g => g.Value)
					.Include(g => g.TeacherId)
					.Include(g => g.Added)
					.AsNoTracking()
					.FirstOrDefaultAsync();
			return gradeAverage;

		}

		public async Task AddGradeAverageForSubjectInstace(GradeAverage gradeAverage) { 


			

            //Grading scale is relative to the country of school
            if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
            {
                throw new ArgumentOutOfRangeException("Grade value");
            }
            grade.AddedBy = usertype;
            await context.Grades.AddAsync(grade);

            if (saveChanges)
            await context.SaveChangesAsync();
		}
		

	}