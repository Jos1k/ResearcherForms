using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearcherForms.BusinessLogic;
using ResearcherForms.Models;

namespace ResearcherForms.Controllers {

	[Authorize( Roles = StaticHelper.RoleNames.Admin )]
	public class AdminController : Controller {
		private IAdminManager _adminManager;

		public AdminController( IAdminManager adminManager ) {
			_adminManager = adminManager;
		}

		public ActionResult CreateCourse( string name ) {
			try {
				long courseId = _adminManager.CreateCourse( name );
				UrlHelper u = new UrlHelper( this.ControllerContext.RequestContext );
				string url = u.Action( "GetCourseInfo", "Admin", new { courseId = courseId }, this.Request.Url.Scheme );
				return Json( url );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

		public ActionResult GetCourseInfo( long courseId ) {
			string courseForAdmin = _adminManager.GetCourseByIdByJSON( courseId );
			ViewBag.Course = courseForAdmin;
			return View( "~/Views/Admin/ManageCourse.cshtml" );
		}
	}
}