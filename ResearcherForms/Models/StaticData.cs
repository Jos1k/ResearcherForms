using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearcherForms.Models {
	public static class StaticData {
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