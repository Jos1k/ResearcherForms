using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResearcherForms.Models {
	public static class StaticHelper {
		public static class RoleNames {
			public const string Admin = "Admin";
			public const string Researcher = "Researcher";
		}

		public static Dictionary<string, int> ControlTypes = new Dictionary<string, int>{
			{"autocomplete",0},
			{"date",1},
			{"checkbox",2},
			{"checkbox-group",3},
			{"radio-group",4},
			{"rich-text",5},
			{"select",6},
			{"text",7}
		};

		public static string RenderPartialViewToString( Controller controller, string viewName, object model ) {
			controller.ViewData.Model = model;
			try {
				using( StringWriter sw = new StringWriter() ) {
					ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView( controller.ControllerContext, viewName );
					ViewContext viewContext = new ViewContext( controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw );
					viewResult.View.Render( viewContext, sw );

					return sw.GetStringBuilder().ToString();
				}
			} catch( Exception ex ) {
				return ex.ToString();
			}
		}

	}
}