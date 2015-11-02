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

		public string GetFormModelByJSON( long formId ) {
			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			var shortForm = new {
				formName = form.Name,
				formId = form.Id,
				fields = form.Fields.OrderBy(field=>field.PositionOnForm).Select(field=>new {
					id = field.Id,
					name = field.Name,
					label = field.Label,
					fieldType = StaticHelper.ControlTypes.FirstOrDefault(controlType=> controlType.Value == field.FieldType).Key,
					required = field.Required,
					description = field.Description,
					position = field.PositionOnForm,
					options = field.Options==null || field.Options.Count==0 
						? null 
						: field.Options.Select(option=>new {
							id = option.Id,
							name = option.Name,
							value = option.Value
						})
				})
			};
			return JsonConvert.SerializeObject( shortForm );
		}
	}
}