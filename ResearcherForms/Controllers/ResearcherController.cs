using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearcherForms.BusinessLogic;
using ResearcherForms.Models;

namespace ResearcherForms.Controllers {

	[Authorize( Roles = StaticHelper.RoleNames.Researcher )]
	public class ResearcherController : Controller {
		private IResearcherManager _researcherManager;

		public ResearcherController( IResearcherManager researcherManager ) {
			_researcherManager = researcherManager;
		}

		public ActionResult GetCourseForms( long courseId ) {
			string courseForResearcher = _researcherManager.GetCourseByIdByJSON( courseId );
			ViewBag.Course = courseForResearcher;
			return View( "~/Views/Researcher/ResearcherCourseForms.cshtml" );
		}

		public ActionResult GetEmptyFormForResearcher( long formId ) {
			return PartialView("~/Views/Researcher/_ResearcherFormModal.cshtml", formId);
		}
	}
}