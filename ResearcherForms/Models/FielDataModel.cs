using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ResearcherForms.Models {
	public class FielData {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string Value { get; set; }
		public bool IsOption { get; set; }
		public int FormFieldId { get; set; }
	}
}
