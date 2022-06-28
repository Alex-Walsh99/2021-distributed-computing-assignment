using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.BindingModels
{
    /// <summary>
    /// Details on a meet-up for swimming events
    /// </summary>
    [DataContract(Name = "meet")]
    public class MeetBindingModel
    {
        /// <summary>
        /// Address line 1
        /// </summary>
        [DataMember(Name = "address_line_1")]
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Address line 2
        /// </summary>
        [DataMember(Name = "address_line_2")]
        public string AddressLine2 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }
        /// <summary>
        /// Postcode
        /// </summary>
        [DataMember(Name = "postcode")]
        [StringLength(10)]
        public string Postcode { get; set; }
        /// <summary>
        /// Date and time of meet
        /// </summary>
        [DataType(DataType.Date)]
        [DataMember(Name = "meet_date_time")]
        public DateTime MeetDateTime { get; set; }
        /// <summary>
        /// Pool size (m)
        /// </summary>
        [DataMember(Name = "pool_size_in_meters")]
        public int PoolSizeInMeters { get; set; }
    }
}