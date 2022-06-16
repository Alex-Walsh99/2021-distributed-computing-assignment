using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "meet")]
    public class MeetViewModel
    {
        [DataMember(Name = "address_line_1")]
        public string AddressLine1 { get; set; }
        [DataMember(Name = "address_line_2")]
        public string AddressLine2 { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "postcode")]
        public string Postcode { get; set; }
        [DataMember(Name = "meet_date_time")]
        public DateTime? MeetDateTime { get; set; }
        [DataMember(Name = "pool_size_in_meters")]
        public int? PoolSizeInMeters { get; set; }

        [DataMember(Name = "events")]
        public List<EventViewModel> Events { get; set; }
    }
}