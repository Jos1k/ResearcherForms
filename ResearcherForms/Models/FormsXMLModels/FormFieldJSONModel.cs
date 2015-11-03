using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearcherForms.Models.FormsXMLModels {
	public class formFieldJSONModel {
		public long formId { get; set; }
		public ICollection<fieldrData> fieldsData { get; set; }
	}
	public class fieldrData {
		public long fieldId { get; set; }
		public object fieldData { get; set; }
		public string fieldType { get; set; }
		public ICollection<optionData> options { get; set; }
	}
	public class optionData {
		public long optionId { get; set; }
		public object value { get; set; }
	}

}