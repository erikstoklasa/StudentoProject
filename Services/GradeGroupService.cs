using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolGradebook.Models.GradeGroup;

namespace SchoolGradebook.Services {
	public class GradeGroupService {
		private readonly SchoolContext context;


		public GradeGroupService(SchoolContext context) {
			this.context = context;

		}
		public async Task<GradeGroup> GetGradeGroupAsync(int gradeGroupId) {

			GradeGroup gradeGroup = await context.GradeGroups
				.Where(s => s.Id == gradeGroupId)
				.AsNoTracking()//what does this do ?
				.FirstOrDefaultAsync();

			return gradeGroup;
		}
	
		
	
	}



}