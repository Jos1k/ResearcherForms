using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResearcherForms.Models {
	[Table( "dbCourses" )]
	public class Course {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
		[ForeignKey( "UserId" )]
		public virtual ApplicationUser User { get; set; }
		public virtual ICollection<ResearchForm> Forms { get; set; }
	}
}