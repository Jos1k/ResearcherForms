﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ResearcherForms.Models;
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
			YAXSerializer ser = new YAXSerializer( typeof( dynamic ) );
			dynamic _formBody = ser.Deserialize( formBody );
	
			ApplicationDbContext dbContext = new ApplicationDbContext();
			ResearchForm form = new ResearchForm(){
				Name = formName,
				ResearchCourseId = courseId
			};
			dbContext.ResearchForms.Add(form);
			dbContext.SaveChanges();

			return JsonConvert.SerializeObject(new {id = form.Id, name = form.Name});
		}
	}
}