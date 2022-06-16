using Microsoft.AspNet.Identity.EntityFramework;
using Sem_2_Swimclub.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Sem_2_Swimclub.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserName { get; set; }

        public List<Competitor> Competitions { get; set; }
    }

    public class UserDetailsViewModel
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserName { get; set; }

        public bool Inactive { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class UserDetailsEditModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }


    public class UserArchiveEditModel
    {
        public bool IsArchived { get; set; }
    }

}
