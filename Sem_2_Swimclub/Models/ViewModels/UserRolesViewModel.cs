using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "role")]
    public class RolesViewModel
    {
        [DataMember(Name = "role_name")]
        public string RoleName { get; set; }

        [DataMember(Name = "users")]
        public List<RolesUserViewModel> RoleUsers { get; set; }
    }

    public class RoleViewModel
    {
        public string RoleUrl { get; set; }
        public string RoleName { get; set; }
    }

    [DataContract(Name = "role_user")]
    public class RolesUserViewModel
    {
        [DataMember(Name = "user_url")]
        public string UserUrl { get; set; }
        [DataMember(Name = "user_email")]
        public string UserEmail { get; set; }
    }
}