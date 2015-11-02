using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ResearcherForms.Models;
using ResearcherForms.Models.FormsXMLModels;
using YAXLib;

namespace ResearcherForms.BusinessLogic {

	public class ResearcherManager : IResearcherManager {
		private ApplicationDbContext _dbContext;
		public ResearcherManager( ApplicationDbContext dbContext ) {
			_dbContext = dbContext;
		}

		public string GetCourseByIdByJSON( long courseId ) {
			Course course = _dbContext.Courses.Find( courseId );
			var courseForResearcher = new {
				id = course.Id,
				name = course.Name,
				forms = course.Forms.Select( form =>
					new {
						id = form.Id,
						name = form.Name
					}
				)
			};
			return JsonConvert.SerializeObject( courseForResearcher );
		}
	}
}