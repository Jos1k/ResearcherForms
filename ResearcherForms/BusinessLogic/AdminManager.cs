using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ResearcherForms.Models;

namespace ResearcherForms.BusinessLogic {

	public class AdminManager:IAdminManager {
		private ApplicationDbContext _dbContext;
		public AdminManager( ApplicationDbContext dbContext ) {
			_dbContext = dbContext;
		}

		public string GetAllCoursesByJSON() {
			var shortCourses = _dbContext.Courses.ToList().Select( x => new {
					Id=x.Id,
					Name=x.Name,
					ClassListCount = x.ClassList.Count(),
					FormsCount = x.Forms.Count()
				} 
			);
			return JsonConvert.SerializeObject( shortCourses );
		}
	}
}