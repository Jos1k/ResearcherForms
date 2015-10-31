using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using ResearcherForms.Models;
using ResearcherForms.Models.FormsXMLModels;

namespace ResearcherForms.BusinessLogic {
	internal static class ParseHelper {
		private static JavaScriptSerializer json;
		private static JavaScriptSerializer JSON { get { return json ?? ( json = new JavaScriptSerializer() ); } }

		public static Stream ToStream( this string @this ) {
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.Write( @this );
			writer.Flush();
			stream.Position = 0;
			return stream;
		}


		public static T ParseXML<T>( this string @this ) where T : class {
			var reader = XmlReader.Create( @this.Trim().ToStream(),
				new XmlReaderSettings() {
					ConformanceLevel = ConformanceLevel.Document
				}
			);
			return new XmlSerializer( typeof( T ) ).Deserialize( reader ) as T;
		}

		public static T ParseJSON<T>( this string @this ) where T : class {
			return JSON.Deserialize<T>( @this.Trim() );
		}

		public static string SerializeResearchFormToXML( ICollection<Field> _fields ) {
			StringBuilder resultSB = new StringBuilder();

			formtemplate formTemplate = new formtemplate() {
				fields = _fields.OrderBy( field => field.PositionOnForm )
				.Select( field => new formtemplateField() {
					description = field.Description,
					label = field.Label,
					name = field.Name,
					required = field.Required,
					style = field.Options != null && field.Options.Count > 0
					? "multiple"
					: null,
					type = StaticHelper.ControlTypes.First(
						controlType => controlType.Value == field.FieldType
					).Key,
					option = field.Options != null && field.Options.Count > 0
					? field.Options.Select(
							option => new formtemplateFieldOption() {
								value = option.Value,
								Value = option.Name
							}
					).ToArray()
					: null
				} )
				.ToArray()
			};

			XmlSerializer xmlSerializer = new XmlSerializer( typeof( formtemplate ) );
			StringWriter writer = new StringWriter( resultSB );
			xmlSerializer.Serialize( writer, formTemplate );
			string result = resultSB.ToString();
			result = result.Substring( result.IndexOf( Environment.NewLine ) + 2 );
			result = result.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

			return result;
		}
	}
}