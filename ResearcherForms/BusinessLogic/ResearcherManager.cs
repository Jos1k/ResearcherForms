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
				fields = form.Fields.OrderBy( field => field.PositionOnForm ).Select( field => new {
					id = field.Id,
					name = field.Name,
					label = field.Label,
					fieldType = StaticHelper.ControlTypes.FirstOrDefault( controlType => controlType.Value == field.FieldType ).Key,
					required = field.Required,
					description = field.Description,
					position = field.PositionOnForm,
					options = field.Options == null || field.Options.Count == 0
						? null
						: field.Options.Select( option => new {
							id = option.Id,
							name = option.Name,
							value = option.Value
						} )
				} )
			};
			return JsonConvert.SerializeObject( shortForm );
		}
		public void FillNewForm( string formModel, string userId ) {
			formFieldJSONModel formFieldData = JsonConvert.DeserializeObject<formFieldJSONModel>( formModel );

			List<FieldData> fieldsAndOptions = formFieldData.fieldsData.Select( fieldData =>
				new FieldData() {
					FormFieldId = fieldData.fieldId,
					IsOption = false,
					Value = GetStringDataFromObject( fieldData.fieldType, fieldData.fieldData )
				}
			).ToList();

			List<optionData> fieldsWithOptions = new List<optionData>();

			formFieldData.fieldsData
			.Where( fieldData => fieldData.options != null )
			.ToList()
			.ForEach( fieldData => fieldsWithOptions.AddRange( fieldData.options.ToList() ) );

			fieldsAndOptions.AddRange(
				fieldsWithOptions.Select( option =>
					new FieldData() {
						FormFieldId = option.optionId,
						IsOption = true,
						Value = ( (bool)option.value ).ToString()
					}
				).ToList()
			);

			UserFormFieldData userFormFieldData = new UserFormFieldData() {
				DateCreating = DateTime.Now,
				ResearchFormId = formFieldData.formId,
				UserId = userId,
				ResearchNumber = _dbContext.ResearchForms.Find( formFieldData.formId ).UserFormsFieldData.Count + 1,
				Options = fieldsAndOptions
			};

			_dbContext.ResearchForms.Find( formFieldData.formId ).UserFormsFieldData.Add( userFormFieldData );
			_dbContext.SaveChanges();
		}

		public string GetFormActivityByJSON( long formId ) {
			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			var shortForm = new {
				name = form.Name,
				id = form.Id,
				courseId = form.ResearchCourseId,
				formActivity = form.UserFormsFieldData.Select( formFIeldData => new {
					formFieldDate = formFIeldData.DateCreating,
					formFieldId = formFIeldData.Id,
					formFieldNumber = formFIeldData.ResearchNumber
				} )
			};
			return JsonConvert.SerializeObject( shortForm );
		}

		private string GetStringDataFromObject( string fieldType, object value ) {
			switch( fieldType ) {
				case "date": return ( (DateTime)value ).ToString();
				case "checkbox": return value == null ? "false" : ( (bool)value ).ToString();
				case "checkbox-group": return null;
				case "radio-group": return ( (long)value ).ToString();
				case "rich-text": return ( value as string );
				case "select": return ( value as string );
				case "text": return ( value as string );
				default: return value.ToString();
			}
		}
	}
}