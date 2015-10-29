using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResearcherForms.BusinessLogic {
	public interface IAdminManager {
		string GetAllCoursesByJSON();
		long CreateCourse(string name);
		string GetCourseByIdByJSON(long courseId);
		string GetExistingUsersNotIncludedInCourse( long courseId );
		void AddExistingUsersToCourse( string[] userIds, long courseId );
		void RemoveExistingUsersToCourse( string[] userIds, long courseId );
	}
}
