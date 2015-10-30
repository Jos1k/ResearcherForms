using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearcherForms.Models.FormsXMLModels {

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
	[System.Xml.Serialization.XmlRootAttribute( "form-template", Namespace = "", IsNullable = false )]
	public partial class formtemplate {

		private formtemplateField[] fieldsField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute( "field", IsNullable = false )]
		public formtemplateField[] fields {
			get {
				return this.fieldsField;
			}
			set {
				this.fieldsField = value;
			}
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
	public partial class formtemplateField {

		private formtemplateFieldOption[] optionField;

		private string nameField;

		private string labelField;

		private string styleField;

		private string descriptionField;

		private bool requiredField;

		private string typeField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute( "option" )]
		public formtemplateFieldOption[] option {
			get {
				return this.optionField;
			}
			set {
				this.optionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string label {
			get {
				return this.labelField;
			}
			set {
				this.labelField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string style {
			get {
				return this.styleField;
			}
			set {
				this.styleField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string description {
			get {
				return this.descriptionField;
			}
			set {
				this.descriptionField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public bool required {
			get {
				return this.requiredField;
			}
			set {
				this.requiredField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string type {
			get {
				return this.typeField;
			}
			set {
				this.typeField = value;
			}
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute( AnonymousType = true )]
	public partial class formtemplateFieldOption {

		private string valueField;

		private string valueField1;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string Value {
			get {
				return this.valueField1;
			}
			set {
				this.valueField1 = value;
			}
		}
	}


}