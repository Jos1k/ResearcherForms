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
			ViewBag.AddExistingUserToCourserModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_AddExistingUserModal", courseId );
			ViewBag.RemoveExistingUserToCourserModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_RemoveExistingUserModal", courseId );
			ViewBag.ChangeCourseNameModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_ChangeCourseNameModal", courseId );
			ViewBag.AddNewUserToCourserModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_AddNewUserModal", courseId );
			ViewBag.AddNewFormToCourserModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_AddNewFormModal", courseId );
			ViewBag.RemoveExistingFormsModalTemplate =
				StaticHelper.RenderPartialViewToString( this, "_RemoveFormModal", courseId );
			return View( "~/Views/Admin/ManageCourse.cshtml" );
		}

		[HttpPost]
		public ActionResult AddExisitingUsersToCourseGetUsers( long courseId ) {
			string users = _adminManager.GetExistingUsersNotIncludedInCourse( courseId );
			return Json( users );
		}

		[HttpPost]
		public ActionResult AddExisitingUsersToCourse( string[] userIds, long courseId ) {
			_adminManager.AddExistingUsersToCourse( userIds, courseId );
			return Json( "" );
		}

		[HttpPost]
		public ActionResult RemoveExisitingUsersFromCourse( string[] userIds, long courseId ) {
			_adminManager.RemoveExistingUsersToCourse( userIds, courseId );
			return Json( "" );
		}

		[HttpPost]
		public ActionResult UpdateCourseName( string courseName, long courseId ) {
			try {
				_adminManager.UpdateCourseName( courseName, courseId );
				return Json( "" );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

		[HttpPost]
		public ActionResult AddOrUpdateNewForm( long courseId, string formName, string formBody , long formId, bool isNew) {
			try {
				string form = "";
				if( isNew ) {
					form = _adminManager.CreateFormByJSON( courseId, formName, formBody );
				} else {
					form = _adminManager.UpdateFormByJSON( formId, formName, formBody );
				}
				return Json( form );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

		[HttpPost]
		public ActionResult RemoveResearchForm( long courseId, long[] formIds ) {
			_adminManager.RemoveResearchForm( courseId, formIds );
			return Json( "" );
		}

		[HttpPost]
		public ActionResult GetExisingFormXML( long formId ) {
			try {
				string xmlForm = _adminManager.GetXmlFormByIdByJSON( formId );
				return Json( xmlForm );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}
	}
}