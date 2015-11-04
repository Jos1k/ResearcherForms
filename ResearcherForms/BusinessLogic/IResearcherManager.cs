using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearcherForms.Models;

namespace ResearcherForms.BusinessLogic {
	public interface IResearcherManager {
		string GetCourseByIdByJSON( long courseId );
		string GetFormModelByJSON( long formId );

		void FillNewForm( string formModel, string userId );
		string GetFormActivityByJSON( long formId );
		string GetFormFieldDataByJSON( long formFieldId );
	}
}
