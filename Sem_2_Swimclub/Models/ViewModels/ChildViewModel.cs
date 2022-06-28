using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    /// <summary>
    /// A child that is part of a family group
    /// </summary>
    [DataContract(Name = "swimmer")]
    public class ChildViewModel
    {
        /// <summary>
        /// URL of family group
        /// </summary>
        [DataMember(Name = "family_group_url")]
        public string FamilyGroupUrl { get; set; }
        /// <summary>
        /// URL of child user
        /// </summary>
        [DataMember(Name = "child_user_url")]
        public string ChildUserUrl { get; set; }

    }
}