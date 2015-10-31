using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ResearcherForms.Models {
	public class Field {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public int FieldType { get; set; }
		public long ResearchFormId { get; set; }
		public int PositionOnForm { get; set; }
		public bool Required { get; set; }
		//[ForeignKey( "ResearchFormId" )]
		public virtual ResearchForm ResearchForm { get; set; }
		public virtual ICollection<Option> Options { get; set; }
	}
}
