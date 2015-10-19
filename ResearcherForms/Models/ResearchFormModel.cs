using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ResearcherForms.Models {
	[Table( "dbResearchForms" )]
	public class ResearchForm {
		[Key]
		[DatabaseGeneratedAttribute( DatabaseGeneratedOption.Identity )]
		public long Id { get; set; }
		public string Name { get; set; }
		public long ResearchCourseId { get; set; }
		[ForeignKey( "ResearchCourseId" )]
		public virtual Course ResearchCourse { get; set; }
		public virtual ICollection<Field> Fields { get; set; }
	}
}
