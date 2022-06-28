using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sem_2_Swimclub.Models.ViewModels
{
    /// <summary>
    /// A role and the users accociated with it.
    /// </summary>
    [DataContract(Name = "role")]
    public class RolesViewModel
    {
        /// <summary>
        /// The name of the role
        /// </summary>
        [DataMember(Name = "role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// The list of users assigned to the role
        /// </summary>
        [DataMember(Name = "users")]
        public List<RolesUserViewModel> RoleUsers { get; set; }
    }

    /// <summary>
    /// Roles that can be linked to a specific user and grant permissions to different requests.
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Role URL
        /// </summary>
        public string RoleUrl { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string RoleName { get; set; }
    }

    /// <summary>
    /// The details of a user assigned to a role
    /// </summary>
    [DataContract(Name = "role_user")]
    public class RolesUserViewModel
    {
        /// <summary>
        /// User details URL
        /// </summary>
        [DataMember(Name = "user_url")]
        public string UserUrl { get; set; }
        /// <summary>
        /// User email address
        /// </summary>
        [DataMember(Name = "user_email")]
        public string UserEmail { get; set; }
    }
}