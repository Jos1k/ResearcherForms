using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ResearcherForms.Models;
using System.Web.Security;
using System.IO;


namespace ResearcherForms {
	public class TestInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext> {
		protected override void Seed( ApplicationDbContext context ) {
			ApplicationUserManager userManager = new ApplicationUserManager( new UserStore<ApplicationUser>( context ) );
			RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( context ) );

			///Creating Roles
			List<IdentityRole> roles = new List<IdentityRole>() 
			{
				new IdentityRole()
				{
					Name =  StaticHelper.RoleNames.Admin
				},
				new IdentityRole()
				{
					Name =  StaticHelper.RoleNames.Researcher
				}
			};
			roles.ForEach( role => roleManager.Create( role ) );

			///Creating Admin User
			var adminUser = new ApplicationUser() {
				Email = "Admin1@admin.com",
				UserName = "Admin1@admin.com"
			};
			userManager.Create( adminUser, adminUser.Email );
			userManager.AddToRole( adminUser.Id, roles.ElementAt( 0 ).Name );

			//Creating basic courses
			List<Course> courses = new List<Course>(){
				new Course()
				{
					Name = "Test Course 1"
				},
				new Course()
				{
					Name = "Test Course 2"
				}
			};
			context.Courses.AddRange( courses );
			context.SaveChanges();

			List<ApplicationUser> users = new List<ApplicationUser>(){
				new ApplicationUser(){
					Email = "Researcher1@researcher.com",
					UserName = "Researcher1@researcher.com",
					Courses = new List<Course>(){
						context.Courses.First()
					}
				},
				new ApplicationUser(){
					Email = "Researcher2@researcher.com",
					UserName = "Researcher2@researcher.com",
					Courses = new List<Course>(){
						context.Courses.First()
					}
				},
				new ApplicationUser(){
					Email = "Researcher3@researcher.com",
					UserName = "Researcher3@researcher.com",
					Courses = new List<Course>(){
						context.Courses.Find(2)
					}
				},
				new ApplicationUser(){
					Email = "Researcher4@researcher.com",
					UserName = "Researcher4@researcher.com"
				},
				new ApplicationUser(){
					Email = "Researcher5@researcher.com",
					UserName = "Researcher5@researcher.com"
				}
			};

			///Creating default researchers
			foreach( var user in users ) {
				IdentityResult identityResult = userManager.Create( user, user.Email );
				while( !identityResult.Succeeded ) {
					identityResult = userManager.Create( user, user.Email );
					if( !identityResult.Succeeded ) {
						string errors = string.Empty;
						identityResult.Errors.Select( err => errors = string.Format( "{0}, {1}", errors, err ) );
						throw new InvalidOperationException( errors );
					}
				}
				userManager.AddToRole( user.Id, StaticHelper.RoleNames.Researcher );
			}

			List<ResearchForm> forms = new List<ResearchForm>() {
				new ResearchForm(){ Name = "Test Form 1", ResearchCourseId = courses[0].Id },
				new ResearchForm(){ Name = "Test Form 2", ResearchCourseId = courses[0].Id },
				new ResearchForm(){ Name = "Test Form 3", ResearchCourseId = courses[0].Id }
			};
			context.ResearchForms.AddRange( forms );
			context.SaveChanges();
		}
	}
}
