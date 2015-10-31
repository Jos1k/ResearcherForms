using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearcherForms.Models;

namespace ResearcherForms.BusinessLogic {
	public interface IAdminManager {
		string GetAllCoursesByJSON();
		long CreateCourse(string name);
		string GetCourseByIdByJSON(long courseId);
		string GetExistingUsersNotIncludedInCourse( long courseId );
		void AddExistingUsersToCourse( string[] userIds, long courseId );
		void RemoveExistingUsersToCourse( string[] userIds, long courseId );
		void UpdateCourseName( string courseName, long courseId );
		void AddUserToCourse( long courseId, string userId );
		string CreateFormByJSON( long courseId, string formName, string formBody );
		void RemoveResearchForm( long courseId, long[] formsId );
		string GetXmlFormByIdByJSON( long formId );
		string UpdateFormByJSON( long formId, string formName, string formBody );
	}
}
