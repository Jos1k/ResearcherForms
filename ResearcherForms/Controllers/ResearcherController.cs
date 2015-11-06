using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearcherForms.BusinessLogic;
using ResearcherForms.Models;
using Microsoft.AspNet.Identity;

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
			ViewBag.BasicUrl = string.Format( "{0}://{1}:{2}", this.Request.Url.Scheme, this.Request.Url.Host, this.Request.Url.Port );
			return View( "~/Views/Researcher/ResearcherCourseForms.cshtml" );
		}

		public ActionResult GetEmptyFormForResearcher( long formId ) {
			ViewBag.FormModel = _researcherManager.GetFormModelByJSON(formId);
			ViewBag.BasicUrl = string.Format( "{0}://{1}:{2}", this.Request.Url.Scheme, this.Request.Url.Host, this.Request.Url.Port );
			return PartialView("~/Views/Researcher/_ResearcherFormModal.cshtml", formId);
		}

		public ActionResult FillNewForm( string formModel ) {
			try {
				string newFormFieldData = _researcherManager.FillNewForm( formModel, User.Identity.GetUserId() );
				return Json( newFormFieldData );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

		public ActionResult GetFormHistory(long formId) {
			ViewBag.Form = _researcherManager.GetFormActivityByJSON( formId );
			ViewBag.BasicUrl = string.Format( "{0}://{1}:{2}", this.Request.Url.Scheme, this.Request.Url.Host, this.Request.Url.Port );
			return View( "~/Views/Researcher/ResearcherFormHistory.cshtml" );
		}

		public ActionResult GetFormFieldHistory( long formFieldId ) {
			try {
				string formFieldData = _researcherManager.GetFormFieldDataByJSON( formFieldId );
				return Json( formFieldData );
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

	}
}