using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Sem_2_Swimclub.Models;
using Sem_2_Swimclub.Models.ViewModels;
using Sem_2_Swimclub.Providers;
using Sem_2_Swimclub.Results;

namespace Sem_2_Swimclub.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/
        [Authorize(Roles = "Club Member")]
        public List<UserDetailsViewModel> GetAccount()
        {
            List<UserDetailsViewModel> users = new List<UserDetailsViewModel>();
            foreach (ApplicationUser user in db.Users.ToList())
            {
                users.Add(
                    new UserDetailsViewModel
                    {
                        UserId = user.Id,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        DateOfBirth = user.DateOfBirth.Date,
                        UserName = user.Email,
                        Inactive = user.Inactive,
                        Roles = getRoleForUsers(user)
                    }
                );
            }
            return users;
        }


        // GET api/Account/1
        public UserDetailsViewModel GetAccount(string userId)
        {
            ApplicationUser user = UserManager.FindById(userId);
            return new UserDetailsViewModel
            {
                UserId = user.Id,
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth.Date,
                UserName = user.Email
            };
        }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            return new UserInfoViewModel
            {
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                UserName = user.Email
            };
        }
        [Authorize(Roles = "Club Member")]
        public List<UserDetailsViewModel> GetSwimmers(string lastName, int? minAge, int? maxAge)
        {
            List<UserDetailsViewModel> usersModel = new List<UserDetailsViewModel>();

            var users = db.Users.AsQueryable();
            if(minAge != null)
                users = users.Where(u => u.DateOfBirth.Year < (DateTime.Now.Year - minAge));
            if (maxAge != null)
                users = users.Where(u => u.DateOfBirth.Year > (DateTime.Now.Year - maxAge));

            if (!String.IsNullOrEmpty(lastName))
                users = users.Where(u => u.LastName.Contains(lastName));

            foreach (ApplicationUser user in users.ToList())
            {
                usersModel.Add(
                    new UserDetailsViewModel
                    {
                        UserId = user.Id,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        DateOfBirth = user.DateOfBirth.Date,
                        UserName = user.Email,
                        Inactive = user.Inactive,
                        Roles = getRoleForUsers(user)
                    }
                );
            }
            return usersModel;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { 
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                UserName = model.Email, 
                Email = model.Email 
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/Register/Swimmer
        [Authorize(Roles = "Club Member")]
        [Route("Register/Swimmer")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterSwimmer(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                UserName = model.Email,
                Email = model.Email
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            UserManager.AddToRole(user.Id, "Swimmer");
            return Ok();
        }

        // POST api/Account/Register/Parent
        [Authorize(Roles = "Club Member")]
        [Route("Register/Parent")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterParent(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                Title = model.Title,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                UserName = model.Email,
                Email = model.Email
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            UserManager.AddToRole(user.Id, "Parent");
            return Ok();
        }

        // POST api/Account/Roles
        [Authorize(Roles = "Club Member")]
        [Route("Roles")]
        public List<RolesViewModel> GetRoles()
        {
            List<RolesViewModel> roles = new List<RolesViewModel>();
            foreach (IdentityRole i_role in db.Roles.ToList())
            {
                roles.Add(
                    new RolesViewModel
                    {
                        RoleName = i_role.Name,
                        RoleUsers = getRoleUsersFor(i_role)
                    }
                );
            }
            return roles;
        }

        // POST api/Account/Role/{id}
        [Authorize(Roles = "Club Member")]
        [Route("Role/{id}")]
        public RolesViewModel GetRole(string id)
        {
            IdentityRole role = db.Roles.Find(id);
            return new RolesViewModel
            {
                RoleName = role.Name,
                RoleUsers = getRoleUsersFor(role)
            };
        }


        private List<RolesUserViewModel> getRoleUsersFor(IdentityRole role)
        {
            List<RolesUserViewModel> roleUsers = new List<RolesUserViewModel>();
            foreach (IdentityUserRole i_user_role in role.Users.ToList())
            {
                roleUsers.Add(
                    new RolesUserViewModel
                    {
                        UserUrl = Url.Link("DefaultApi", new { controller = "Account", id = i_user_role.UserId}),
                        UserEmail = GetAccount(i_user_role.UserId).UserName
                    }
                );

            }
            return roleUsers;
        }

        private List<RoleViewModel> getRoleForUsers(IdentityUser user)
        {
            List<RoleViewModel> roles = new List<RoleViewModel>();
            foreach (IdentityUserRole i_role in user.Roles.ToList())
            {
                IdentityRole role = db.Roles.Find(i_role.RoleId);
                roles.Add(
                    new RoleViewModel
                    {
                        RoleUrl = Url.Link("DefaultApi", new { controller = "Account/Role", id = role.Id }),
                        RoleName = role.Name
                    }
                );

            }
            return roles;
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }


        // PUT: api/Account/EditDetails/1
        [Authorize(Roles = "Club Member")]
        [Route("Account/EditDetails/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEditDetails(string id, UserDetailsEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user != null)
            {
                user.FirstName = editModel.FirstName;
                user.LastName = editModel.LastName;
            }
            else
                return NotFound();

            db.Entry(user).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // PUT: api/Account/Archive/1
        [Authorize(Roles = "Club Member")]
        [Route("Archive/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutArchive(string id, UserArchiveEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user != null)
            {
                user.Inactive = editModel.IsArchived;
            }
            else
                return NotFound();

            db.Entry(user).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
