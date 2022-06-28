using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    /// <summary>
    /// A group containing a parent and list of children
    /// </summary>
    [DataContract(Name = "family_group")]
    public class FamilyGroupViewModel
    {
        /// <summary>
        /// URL of family group controller
        /// </summary>
        [DataMember(Name = "parent_url")]
        public string ParentUrl { get; set; }
        /// <summary>
        /// Family phone number
        /// </summary>
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Family email address
        /// </summary>
        [DataMember(Name = "email_address")]
        public string Email { get; set; }
        /// <summary>
        /// Family address line 1 
        /// </summary>
        [DataMember(Name = "address_line_1")]
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Family address line 2
        /// </summary>
        [DataMember(Name = "address_line_2")]
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Family address city
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }
        /// <summary>
        /// Family address postcode
        /// </summary>
        [DataMember(Name = "post_code")]
        public string PostCode { get; set; }

        /// <summary>
        /// List of children associated with this group
        /// </summary>
        [DataMember(Name = "children")]
        public List<ChildViewModel> Children { get; set; }
    }

    /// <summary>
    /// Edit details of a family group
    /// </summary>
    [DataContract(Name = "family_group_edit")]
    public class FamilyGroupEditViewModel
    {
        /// <summary>
        /// Amend the existing phone number
        /// </summary>
        [DataMember(Name = "phone_number")]
        public string PhoneNumber { get; set; }

    }
}