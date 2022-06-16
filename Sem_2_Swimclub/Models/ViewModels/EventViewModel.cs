using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "event")]
    public class EventViewModel
    {
        [DataMember(Name = "meet_url")]
        public string MeetUrl { get; set; }
        [DataMember(Name = "age_range")]
        public string AgeRange { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "distace_in_meters")]
        public int? DistanceInMeters { get; set; }
        [DataMember(Name = "lanes")]
        public int? Lanes { get; set; }
        [DataMember(Name = "stroke")]
        public string Stroke { get; set; }
        [DataMember(Name = "round")]
        public string Round { get; set; }
        [DataMember(Name = "date_and_time")]
        public DateTime? EventDateTime { get; set; }

        [DataMember(Name = "competitors")]
        public List<CompetitorViewModel> Competitors { get; set; }
    }

    [DataContract(Name = "event_edit")]
    public class EventEditViewModel
    {
        [DataMember(Name = "date_and_time")]
        public DateTime? EventDateTime { get; set; }
    }
}