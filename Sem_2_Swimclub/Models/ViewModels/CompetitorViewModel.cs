using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    [DataContract(Name = "competitor")]
    public class CompetitorViewModel
    {
        [DataMember(Name = "event_url")]
        public string EventUrl { get; set; }
        [DataMember(Name = "swimmer_url")]
        public string SwimmerUrl { get; set; }
        [DataMember(Name = "lane")]
        public int? Lane { get; set; }
        [DataMember(Name = "time_in_seconds")]
        public double? TimeInSeconds { get; set; }
        [DataMember(Name = "reason_not_finished")]
        public string ReasonNotFinished { get; set; }
    }

    [DataContract(Name = "competition")]
    public class UserCompetitionViewModel
    {
        [DataMember(Name = "event_url")]
        public string EventUrl { get; set; }
        [DataMember(Name = "swimmer_url")]
        public string SwimmerUrl { get; set; }
        [DataMember(Name = "meet_url")]
        public string MeetUrl { get; set; }
        [DataMember(Name = "lane")]
        public int? Lane { get; set; }
        [DataMember(Name = "time_in_seconds")]
        public double? TimeInSeconds { get; set; }
        [DataMember(Name = "reason_not_finished")]
        public string ReasonNotFinished { get; set; }
    }
}