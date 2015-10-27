using System.Web;
using System.Web.Optimization;

namespace ResearcherForms {
	public class BundleConfig {
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles( BundleCollection bundles ) {
			bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery-ui.js",
						"~/Scripts/form-builder.js" ) );

			bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
						"~/Scripts/jquery.validate*" ) );

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
						"~/Scripts/modernizr-*" ) );

			bundles.Add( new ScriptBundle( "~/bundles/bootstrap" ).Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js",
					  "~/Scripts/ui-bootstrap-tpls-0.14.3.min.js" ) );

			bundles.Add( new StyleBundle( "~/Content/css" ).Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css",
					  "~/Content/jquery-ui.css",
					  "~/Content/form-builder.css",
					  "~/Content/font-awesome.min.css" ) );

			bundles.Add( new ScriptBundle( "~/bundles/ResearchersAngularModule" )
			   .IncludeDirectory( "~/Scripts/angularControllers", "*.js" )
			   .Include( "~/Scripts/researchersAppModule.js" ) );
		}
	}
}
