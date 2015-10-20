﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ResearcherForms.Models {
	public class Option {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public long FormFieldId { get; set; }
		//[ForeignKey( "FormFieldId" )]
		public virtual Field FormField { get; set; }
	}
}
