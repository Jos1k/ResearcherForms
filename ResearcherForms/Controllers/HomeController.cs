using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearcherForms.BusinessLogic;
using ResearcherForms.Models;

namespace ResearcherForms.Controllers {
	public class HomeController : Controller {
		private IAdminManager _adminManager;

		public HomeController( IAdminManager adminManager ) {
			_adminManager = adminManager;
		}

		public ActionResult Index() {
			if( !User.Identity.IsAuthenticated ) {
				return RedirectToAction( "Login", "Account" );
			}

			if( User.IsInRole( StaticHelper.RoleNames.Admin) ) {
				return RedirectToAction( "IndexAdmin" );
			} else if( User.IsInRole( StaticHelper.RoleNames.Researcher ) ) {
				return RedirectToAction( "IndexResearcher" );
			}
			return View();
		}

		[Authorize( Roles = StaticHelper.RoleNames.Admin )]
		public ActionResult IndexAdmin() {
			ViewBag.CourseList = _adminManager.GetAllCoursesByJSON();
			ViewBag.CreateCourseModalTemplate = StaticHelper.RenderPartialViewToString( this, "_CreateCourseModal", null );
			ViewBag.BasicUrl = string.Format( "{0}://{1}:{2}", this.Request.Url.Scheme, this.Request.Url.Host, this.Request.Url.Port );
			return View( "~/Views/Home/IndexAdmin.cshtml" );
		}

		[Authorize( Roles = StaticHelper.RoleNames.Researcher )]
		public ActionResult IndexResearcher() {
			throw new NotImplementedException();
		}
	}
}