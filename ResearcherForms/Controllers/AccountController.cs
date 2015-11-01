using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ResearcherForms.BusinessLogic;
using ResearcherForms.Models;

namespace ResearcherForms.Controllers {
	[Authorize]
	public class AccountController : Controller {
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private IAdminManager _adminManager;

		public AccountController( IAdminManager adminManager ) {
			_adminManager = adminManager;
		}

		public AccountController( ApplicationUserManager userManager, ApplicationSignInManager signInManager ) {
			UserManager = userManager;
			SignInManager = signInManager;

		}

		public ApplicationSignInManager SignInManager {
			get {
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set {
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager {
			get {
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set {
				_userManager = value;
			}
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login( string returnUrl ) {
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login( LoginViewModel model, string returnUrl ) {
			if( !ModelState.IsValid ) {
				return View( model );
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			ApplicationUser loginUser = UserManager.FindByEmail( model.Email );
			if( loginUser == null ) {
				ModelState.AddModelError( "", "Invalid login attempt." );
				return View( model );
			}

			var result = await SignInManager.PasswordSignInAsync( loginUser.UserName, model.Password, model.RememberMe, shouldLockout: false );
			switch( result ) {
				case SignInStatus.Success:
					return RedirectToLocal( returnUrl );
				case SignInStatus.LockedOut:
					return View( "Lockout" );
				case SignInStatus.RequiresVerification:
					return RedirectToAction( "SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe } );
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError( "", "Invalid login attempt." );
					return View( model );
			}
		}

		[HttpPost]
		[Authorize( Roles = StaticHelper.RoleNames.Admin )]
		public ActionResult CreateNewUserOnCourse( string userName, string userEmail, string userPassword, long courseId ) {
			try {
				var user = new ApplicationUser { UserName = userName, Email = userEmail };
				var result = UserManager.Create( user, userPassword );
				if( result.Succeeded ) {
					_adminManager.AddUserToCourse( courseId, user.Id );
					result = UserManager.AddToRole( user.Id, StaticHelper.RoleNames.Researcher );
					if( result == null || !result.Succeeded ) {
						AddErrors( result );
						return new HttpStatusCodeResult( 500, result.Errors.First() );
					}
					return Json( user.Id );
				} else {
					return new HttpStatusCodeResult( 500, result.Errors.First() );
				}
			} catch( Exception ex ) {
				return new HttpStatusCodeResult( 500, ex.Message );
			}
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff() {
			AuthenticationManager.SignOut( DefaultAuthenticationTypes.ApplicationCookie );
			return RedirectToAction( "Index", "Home" );
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager {
			get {
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors( IdentityResult result ) {
			foreach( var error in result.Errors ) {
				ModelState.AddModelError( "", error );
			}
		}

		private ActionResult RedirectToLocal( string returnUrl ) {
			if( Url.IsLocalUrl( returnUrl ) ) {
				return Redirect( returnUrl );
			}
			return RedirectToAction( "Index", "Home" );
		}

		internal class ChallengeResult : HttpUnauthorizedResult {
			public ChallengeResult( string provider, string redirectUri )
				: this( provider, redirectUri, null ) {
			}

			public ChallengeResult( string provider, string redirectUri, string userId ) {
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult( ControllerContext context ) {
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if( UserId != null ) {
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge( properties, LoginProvider );
			}
		}
		#endregion
	}
}