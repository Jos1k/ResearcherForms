using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ResearcherForms.Models;

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
	}
}