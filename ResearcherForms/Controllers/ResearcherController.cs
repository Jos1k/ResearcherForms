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

		//public ActionResult CreateCourse( string name ) {
		//	try {
		//		long courseId = _adminManager.CreateCourse( name );
		//		UrlHelper u = new UrlHelper( this.ControllerContext.RequestContext );
		//		string url = u.Action( "GetCourseInfo", "Admin", new { courseId = courseId }, this.Request.Url.Scheme );
		//		return Json( url );
		//	} catch( Exception ex ) {
		//		return new HttpStatusCodeResult( 500, ex.Message );
		//	}
		//}

		public ActionResult GetCourseForms( long courseId ) {
			string courseForResearcher = _researcherManager.GetCourseByIdByJSON( courseId );
			ViewBag.Course = courseForResearcher;
			return View( "~/Views/Researcher/ResearcherCourseForms.cshtml" );
		}
	}
}