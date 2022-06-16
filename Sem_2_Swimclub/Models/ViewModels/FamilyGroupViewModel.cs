using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "family_group")]
    public class FamilyGroupViewModel
    {
        [DataMember(Name = "parent_url")]
        public string ParentUrl { get; set; }
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }
        [DataMember(Name = "email_address")]
        public string Email { get; set; }
        [DataMember(Name = "address_line_1")]
        public string AddressLine1 { get; set; }
        [DataMember(Name = "address_line_2")]
        public string AddressLine2 { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "post_code")]
        public string PostCode { get; set; }

        [DataMember(Name = "children")]
        public List<ChildViewModel> Children { get; set; }
    }

    [DataContract(Name = "family_group_edit")]
    public class FamilyGroupEditViewModel
    {
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }

    }
}