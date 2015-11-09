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
					dataValue = "",
					options = field.Options == null || field.Options.Count == 0
						? null
						: field.Options.Select( option => new {
							id = option.Id,
							name = option.Name,
							value = option.Value,
							isSelected = false
						} )
				} )
			};
			return JsonConvert.SerializeObject( shortForm );
		}


		public string FillNewForm( string formModel, string userId ) {
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

			var shortFormFieldData = new {
				formFieldDate = userFormFieldData.DateCreating,
				formFieldId = userFormFieldData.Id,
				formFieldNumber = userFormFieldData.ResearchNumber
			};

			return JsonConvert.SerializeObject( shortFormFieldData );
		}

		public string GetFormActivityByJSON( long formId ) {
			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			var shortForm = new {
				name = form.Name,
				id = form.Id,
				courseId = form.ResearchCourseId,
				formActivity = form.UserFormsFieldData
					.OrderByDescending( formFIeldData => formFIeldData.ResearchNumber )
					.Select( formFIeldData => new {
						formFieldDate = formFIeldData.DateCreating,
						formFieldId = formFIeldData.Id,
						formFieldNumber = formFIeldData.ResearchNumber
					} ),
				formModel = JsonConvert.DeserializeObject( GetFormModelByJSON( formId ) )
			};
			return JsonConvert.SerializeObject( shortForm );
		}

		public string GetFormFieldDataByJSON( long formFieldId ) {
			List<FieldData> formFieldDataOptions = _dbContext.UserFormsFieldData.Find( formFieldId ).Options.ToList();
			var shortFormFieldData = formFieldDataOptions.Select( fieldData =>
				new {
					value = fieldData.Value,
					fieldId = fieldData.FormFieldId,
					isOption = fieldData.IsOption
				}
			).ToList();
			return JsonConvert.SerializeObject( shortFormFieldData );
		}

		private string GetStringDataFromObject( string fieldType, object value ) {
			switch( fieldType ) {
				case "date": return ( value as string );
				case "checkbox": return string.IsNullOrEmpty(
					value.ToString()
					)
					? "false"
					: ( (bool)value ).ToString();
				case "checkbox-group": return null;
				case "radio-group": return ( value is long ) ? ( (long)value ).ToString() : "";
				case "rich-text": return ( value as string );
				case "select": return ( value as string );
				case "text": return ( value as string );
				default: return value.ToString();
			}
		}

		public List<FormAnalyticFormModalHeader> GetFormAnalyticHeaderByJSON( long formId ) {
			List<FormAnalyticFormModalHeader> headers = new List<FormAnalyticFormModalHeader>();

			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			headers.AddRange(
				form
				.Fields
				.OrderBy( field => field.PositionOnForm ).Select(
					field => new FormAnalyticFormModalHeader {
						FieldName = field.Label,
						Options = field.FieldType == StaticHelper.ControlTypes["checkbox-group"]
						&& ( field.Options != null && field.Options.Count > 0 )
							? field.Options.Select( option => option.Name ).ToList()
							: null
					}
				).ToList()
			);
			return headers;
		}

		private List<dynamic> GetFieldsDataInCorrectOrder( UserFormFieldData formFieldData ) {
			var formFieldsDataOrder = formFieldData.ResearchForm.Fields.OrderBy( x => x.PositionOnForm ).ToList();

			List<Tuple<long, bool, bool>> fieldIdInCorrectOrder = new List<Tuple<long, bool, bool>>();
			formFieldsDataOrder.ForEach( field => {
				if( field.Options == null || field.Options.Count == 0 ) {
					fieldIdInCorrectOrder.Add( new Tuple<long, bool, bool>( field.Id, false, false ) );
				} 
				if(
					  field.FieldType != StaticHelper.ControlTypes["checkbox-group"]
					  && ( field.Options != null && field.Options.Count > 0 )
				  ) {
					fieldIdInCorrectOrder.Add( new Tuple<long, bool, bool>( field.Id, false, true ) );
				} 
				if(
					  field.FieldType == StaticHelper.ControlTypes["checkbox-group"]
					  && ( field.Options != null && field.Options.Count > 0 )
				  ) {
					field.Options.ToList().ForEach( option =>
						fieldIdInCorrectOrder.Add( new Tuple<long, bool, bool>( option.Id, true, false ) )
					);
				}
			} );

			List<dynamic> fields = new List<dynamic>();

			fieldIdInCorrectOrder.ForEach( fieldInForm => {
				if( !fieldInForm.Item3 ) {
					FieldData fieldData = formFieldData
					.Options
					.FirstOrDefault( field =>
						field.IsOption == fieldInForm.Item2
						&& field.FormFieldId == fieldInForm.Item1
					);

					fields.Add( new {
						value = fieldData == null ? null : fieldData.Value,
						fieldId = fieldInForm.Item1
					} );
				} else {
					FieldData selectedOption = formFieldData
						.Options
						.FirstOrDefault( field =>
							!field.IsOption
							&& field.FormFieldId == fieldInForm.Item1
						);
					if( selectedOption != null ) {
						long selectedOptionId;
						bool isNotEmpty = long.TryParse( selectedOption.Value, out selectedOptionId );
						fields.Add( new {
							value = isNotEmpty ? _dbContext.Options.Find( selectedOptionId ).Name:null,
							fieldId = fieldInForm.Item1
						} );
					} else {
						fields.Add( new {
							value = "",
							fieldId = fieldInForm.Item1
						} );
					}
				}
			} );
			return fields;
		}

		public string GetFormAnalyticByJSON( long formId ) {
			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			var shortForm = form.UserFormsFieldData
					.OrderByDescending( formFIeldData => formFIeldData.ResearchNumber )
					.Select(
						formFIeldData => new {
							formFieldDate = formFIeldData.DateCreating,
							formFieldId = formFIeldData.Id,
							formFieldNumber = formFIeldData.ResearchNumber,
							fields = GetFieldsDataInCorrectOrder( formFIeldData )
							//fields = formFIeldData.Options.Select( fieldData =>
							//	new {
							//		value = fieldData.Value,
							//		fieldId = fieldData.FormFieldId,
							//		isOption = fieldData.IsOption
							//	}
							//).ToList()
						}
					).ToList();
			return JsonConvert.SerializeObject( shortForm );
		}
	}
}