using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace ResearcherForms.Models {
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser {

		public virtual ICollection<Course> Courses { get; set; }

		public virtual ICollection<UserFormFieldData> UserFormsFieldData { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<ApplicationUser> manager ) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync( this, DefaultAuthenticationTypes.ApplicationCookie );
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
		public ApplicationDbContext()
			: base( "DefaultConnection", throwIfV1Schema: false ) {
		}
		protected override void OnModelCreating( DbModelBuilder modelBuilder ) {
			base.OnModelCreating( modelBuilder );

			modelBuilder.Entity<ResearchForm>()
				.HasRequired( m => m.ResearchCourse )
				.WithMany( t => t.Forms )
				.HasForeignKey( m => m.ResearchCourseId )
				.WillCascadeOnDelete( false );

			modelBuilder.Entity<Field>()
				.HasRequired( m => m.ResearchForm )
				.WithMany( t => t.Fields )
				.HasForeignKey( m => m.ResearchFormId );

			modelBuilder.Entity<Option>()
				.HasRequired( m => m.FormField )
				.WithMany( t => t.Options )
				.HasForeignKey( m => m.FormFieldId );

			modelBuilder.Entity<UserFormFieldData>()
				.HasRequired( m => m.User )
				.WithMany( t => t.UserFormsFieldData )
				.HasForeignKey( m => m.UserId );

			modelBuilder.Entity<UserFormFieldData>()
				.HasRequired( m => m.ResearchForm )
				.WithMany( t => t.UserFormsFieldData )
				.HasForeignKey( m => m.ResearchFormId );
		}


		public DbSet<Field> Fields { get; set; }
		public DbSet<FieldData> FieldsData { get; set; }
		public DbSet<UserFormFieldData> UserFormsFieldData { get; set; }
		public DbSet<Option> Options { get; set; }
		public DbSet<ResearchForm> ResearchForms { get; set; }
		public DbSet<Course> Courses { get; set; }

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}
	}
}