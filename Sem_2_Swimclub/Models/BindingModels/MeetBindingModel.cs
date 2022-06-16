using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.BindingModels
{
    [DataContract(Name = "meet")]
    public class MeetBindingModel
    {
        [DataMember(Name = "address_line_1")]
        public string AddressLine1 { get; set; }
        [DataMember(Name = "address_line_2")]
        public string AddressLine2 { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "postcode")]
        [StringLength(10)]
        public string Postcode { get; set; }
        [DataType(DataType.Date)]
        [DataMember(Name = "meet_date_time")]
        public DateTime MeetDateTime { get; set; }
        [DataMember(Name = "pool_size_in_meters")]
        public int PoolSizeInMeters { get; set; }
    }
}