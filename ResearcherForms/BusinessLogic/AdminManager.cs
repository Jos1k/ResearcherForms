﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ResearcherForms.Models;
using ResearcherForms.Models.FormsXMLModels;
using YAXLib;

namespace ResearcherForms.BusinessLogic {

	public class AdminManager : IAdminManager {
		private ApplicationDbContext _dbContext;
		public AdminManager( ApplicationDbContext dbContext ) {
			_dbContext = dbContext;
		}

		public string GetAllCoursesByJSON() {
			var shortCourses = _dbContext.Courses.ToList().Select( course => new {
				Id = course.Id,
				Name = course.Name,
				ClassListCount = course.ClassList.Count(),
				FormsCount = course.Forms.Count()
			}
			);
			return JsonConvert.SerializeObject( shortCourses );
		}

		public long CreateCourse( string name ) {

			if( name.Length < 3 || name.Length > 25 ) {
				throw new Exception( "Course should consist at least 3, but not more then 25 letters" );
			} else if( _dbContext.Courses.Any( course =>
						  course.Name.ToLower() == name.ToLower()
					  )
				  ) {
				throw new Exception( "Course with such name already exist" );
			}

			Course resultCourse = new Course();
			resultCourse.Name = name;
			_dbContext.Courses.Add( resultCourse );
			_dbContext.SaveChanges();

			return resultCourse.Id;
		}

		public string GetCourseByIdByJSON( long courseId ) {

			Course course = _dbContext.Courses.Find( courseId );
			var courseForAdmin = new {
				id = course.Id,
				name = course.Name,
				researchers = course.ClassList.Select( researcher =>
					new {
						id = researcher.Id,
						name = researcher.UserName
					}
				),
				forms = course.Forms.Select( form =>
					new {
						id = form.Id,
						name = form.Name
					}
				)
			};
			return JsonConvert.SerializeObject( courseForAdmin );
		}

		public string GetExistingUsersNotIncludedInCourse( long courseId ) {
			var usersInCourse = _dbContext.Courses.Find( courseId ).ClassList.ToList();
			var usersNotInCourse = _dbContext.Users
				.ToList()
				.Where(
				researcher =>
					!usersInCourse.Contains( researcher )
					&& _dbContext.Roles.First(
						role => role.Name == StaticHelper.RoleNames.Admin
					)
					.Users.Any( x => x.UserId != researcher.Id )
				)
				.Select(
					researcher => new {
						id = researcher.Id,
						name = researcher.UserName,
						email = researcher.Email
					}
				)
				.ToList();

			return JsonConvert.SerializeObject( usersNotInCourse );
		}

		public void AddExistingUsersToCourse( string[] userIds, long courseId ) {
			Course course = _dbContext.Courses.Find( courseId );
			userIds.ToList().ForEach( userId => _dbContext.Users.Find( userId ).Courses.Add( course ) );
			_dbContext.SaveChanges();
		}

		public void RemoveExistingUsersToCourse( string[] userIds, long courseId ) {
			Course course = _dbContext.Courses.Find( courseId );
			userIds.ToList().ForEach( userId => _dbContext.Users.Find( userId ).Courses.Remove( course ) );
			_dbContext.SaveChanges();
		}

		public void UpdateCourseName( string courseName, long courseId ) {

			Course course = _dbContext.Courses.Find( courseId );
			if( course.Name.ToLower() == courseName.ToLower() ) {
				course.Name = courseName;
				_dbContext.SaveChanges();
				return;
			} else if( courseName.Length < 3 || courseName.Length > 25 ) {
				throw new Exception( "Course should consist at least 3, but not more then 25 letters" );
			} else if( _dbContext.Courses.Any( courseL =>
						  courseL.Name.ToLower() == courseName.ToLower()
					  )
				  ) {
				throw new Exception( "Course with such name already exist" );
			}
			course.Name = courseName;
			_dbContext.SaveChanges();
		}

		public void AddUserToCourse( long courseId, string userId ) {
			ApplicationDbContext dbContext = new ApplicationDbContext();
			Course course = dbContext.Courses.Find( courseId );
			ApplicationUser user = dbContext.Users.Find( userId );
			course.ClassList.Add( user );
			dbContext.SaveChanges();
		}

		public string CreateFormByJSON( long courseId, string formName, string formBody ) {

			if( formName.Length < 5 || formName.Length > 25 ) {
				throw new Exception( "Form name should consist at least 5, but not more then 25 letters" );
			} else if( _dbContext.ResearchForms.Any( formL =>
						  formL.Name.ToLower() == formName.ToLower()
						  && formL.ResearchCourseId == courseId
					  )
				  ) {
				throw new Exception( "Form with such name already exist in this course!" );
			}

			formtemplate _formBody = formBody.ParseXML<formtemplate>();

			if( _formBody.fields == null || _formBody.fields.Count() == 0 ) {
				throw new Exception( "Form could not be empty!" );
			}

			List<Field> fields = _formBody.fields.ToList().Select( ( field, index ) =>
				new Field() {
					Name = field.name,
					Description = field.description,
					FieldType = StaticHelper.ControlTypes[field.type],
					Label = field.label,
					Required = field.required,
					PositionOnForm = index,
					Options = field.option == null
					? null
					: field.option.Select( option =>
						new Option() {
							Name = option.Value,
							Value = option.value
						}
					)
					.ToList()
				}
			).ToList();

			ApplicationDbContext dbContext = new ApplicationDbContext();
			ResearchForm form = new ResearchForm() {
				Name = formName,
				ResearchCourseId = courseId,
				Fields = fields
			};
			dbContext.ResearchForms.Add( form );
			dbContext.SaveChanges();

			return JsonConvert.SerializeObject( new { id = form.Id, name = form.Name } );
		}

		public void RemoveResearchForm( long courseId, long[] formsId ) {

			ApplicationDbContext dbContext = new ApplicationDbContext();
			Course course = dbContext.Courses.Find( courseId );
			List<ResearchForm> forms = course.Forms.Where(
				form => formsId.Contains( form.Id )
			).ToList();

			forms.ForEach(
				form => dbContext.ResearchForms.Remove( form )
			);
			dbContext.SaveChanges();
		}

		public string GetXmlFormByIdByJSON( long formId ) {
			ResearchForm form = _dbContext.ResearchForms.Find( formId );
			return ParseHelper.SerializeResearchFormToXML( form.Fields );
		}

		public string UpdateFormByJSON( long formId, string formName, string formBody ) {
			ApplicationDbContext dbContext = new ApplicationDbContext();
			ResearchForm updateForm = dbContext.ResearchForms.Find( formId );
			if( formName.Length < 5 || formName.Length > 25 ) {
				throw new Exception( "Form name should consist at least 5, but not more then 25 letters" );
			} else if( updateForm.Name.ToLower() != formName.ToLower()
				&& _dbContext.ResearchForms.Any( formL =>
					formL.Name.ToLower() == formName.ToLower()
					&& formL.ResearchCourseId == updateForm.ResearchCourseId
				)
			) {
				throw new Exception( "Form with such name already exist" );
			}

			formtemplate _formBody = formBody.ParseXML<formtemplate>();

			if( _formBody.fields == null || _formBody.fields.Count() == 0 ) {
				throw new Exception( "Form could not be empty!" );
			}

			List<Field> fields = _formBody.fields.ToList().Select( ( field, index ) =>
				new Field() {
					Name = field.name,
					Description = field.description,
					FieldType = StaticHelper.ControlTypes[field.type],
					Label = field.label,
					Required = field.required,
					PositionOnForm = index,
					Options = field.option == null
					? null
					: field.option.Select( option =>
						new Option() {
							Name = option.Value,
							Value = option.value
						}
					)
					.ToList()
				}
			).ToList();

			List<string> fielsIdsList = fields.Select( field => field.Name ).ToList();
			List<Field> removedList = updateForm.Fields.Where( field => !fielsIdsList.Contains( field.Name ) ).ToList();
			dbContext.Fields.RemoveRange( removedList );

			foreach( Field field in fields ) {
				Field exisitingField = updateForm.Fields.SingleOrDefault( extField => extField.Name == field.Name );
				if( exisitingField == null ) {
					updateForm.Fields.Add( field );
				} else {
					exisitingField.Description = field.Description;
					exisitingField.Label = field.Label;
					exisitingField.Required = field.Required;
					exisitingField.PositionOnForm = field.PositionOnForm;
					if( exisitingField.Options != null && exisitingField.Options.Count > 1 ) {
						List<string> options = field.Options.Select( option => option.Name ).ToList();
						List<Option> removedOptions = exisitingField.Options.Where( option =>
							!field.Options.Any(
								newOption =>
									newOption.Name == option.Name
							)
						).ToList();
						dbContext.Options.RemoveRange( removedOptions );
						List<Option> existingOptions = exisitingField.Options.Where( option => options.Contains( option.Name ) ).ToList();
						existingOptions.ForEach( existingOption =>
							existingOption.Value = field.Options.Single(
								newOption => newOption.Name == existingOption.Name
							).Value
						);
						List<Option> newOptions = field.Options.Where( option =>
							!existingOptions.Any(
								existingOption => existingOption.Name == option.Name
							)
						).ToList();
						foreach( Option option in newOptions ) {
							if( !existingOptions.Any( x => x.Name == option.Name ) ) {
								exisitingField.Options.Add( option );
							} else {
								throw new Exception( "There are could not be 2 options with same name!" );
							}
						};
					}
				}
			};

			updateForm.Name = formName;
			dbContext.SaveChanges();

			return JsonConvert.SerializeObject( new { id = updateForm.Id, name = updateForm.Name } );
		}

		public void RemoveCourse( long courseId ) {
			ApplicationDbContext dbContext = new ApplicationDbContext();
			Course course = dbContext.Courses.Find( courseId );
			if( course.Forms != null && course.Forms.Count > 0 ) {
				dbContext.ResearchForms.RemoveRange( course.Forms );
				dbContext.SaveChanges();
			}

			dbContext.Courses.Remove( course );
			dbContext.SaveChanges();
		}

		public string GetAllCoursesForResearcherByJSON( string researcherId ) {
			ApplicationUser researcher = _dbContext.Users.Find( researcherId );
			if( researcher.Courses != null && researcher.Courses.Count > 0 ) {
				var shortCourses = researcher.Courses.ToList().Select( course => new {
					Id = course.Id,
					Name = course.Name,
					ClassListCount = course.ClassList.Count(),
					FormsCount = course.Forms.Count()
				} );
				return JsonConvert.SerializeObject( shortCourses );
			}
			return "";
		}
	}
}