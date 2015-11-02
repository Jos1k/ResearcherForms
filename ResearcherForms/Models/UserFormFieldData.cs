using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ResearcherForms.Models {
	public class UserFormFieldData {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string UserId { get; set; }
		public DateTime DateCreating { get; set; }
		public int ResearchNumber { get; set; }
		//[ForeignKey( "UserId" )]
		public virtual ApplicationUser User { get; set; }
		public long ResearchFormId { get; set; }
		//[ForeignKey( "ResearchFormId" )]
		public virtual ResearchForm ResearchForm { get; set; }

		public virtual ICollection<FieldData> Options { get; set; }
	}
}
