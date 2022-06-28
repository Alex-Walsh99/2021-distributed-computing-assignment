using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sem_2_Swimclub.Models.ViewModels
{
    /// <summary>
    /// A meet-up for events
    /// </summary>
    [DataContract(Name = "meet")]
    public class MeetViewModel
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
        public string Postcode { get; set; }
        /// <summary>
        /// Date and time of meet
        /// </summary>
        [DataMember(Name = "meet_date_time")]
        public DateTime? MeetDateTime { get; set; }
        /// <summary>
        /// Pool size (m)
        /// </summary>
        [DataMember(Name = "pool_size_in_meters")]
        public int? PoolSizeInMeters { get; set; }

        /// <summary>
        /// List of events that will be taking place at this meet-up
        /// </summary>
        [DataMember(Name = "events")]
        public List<EventViewModel> Events { get; set; }
    }
}