using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "swimmer")]
    public class ChildViewModel
    {
        [DataMember(Name = "family_group_url")]
        public string FamilyGroupUrl { get; set; }
        [DataMember(Name = "child_user_url")]
        public string ChildUserUrl { get; set; }

    }
}