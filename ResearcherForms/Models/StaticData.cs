using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearcherForms.Models {
	public static class StaticData {
		public static class RoleNames {
			public const string Admin = "Admin";
			public const string Researcher = "Researcher";
		}

		public enum ControlTypes {
			DateField,
			Checkbox,
			CheckboxGroup,
			RadioGroup,
			RichTextEditor,
			SelectField,
			TextField
		}
	}
}