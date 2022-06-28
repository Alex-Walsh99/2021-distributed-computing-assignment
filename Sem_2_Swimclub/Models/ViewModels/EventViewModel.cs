using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sem_2_Swimclub.Models.ViewModels
{
    /// <summary>
    /// Details on an event that takes place at a meet-up
    /// </summary>
    [DataContract(Name = "event")] 
    public class EventViewModel
    {
        /// <summary>
        /// URL of the meet associated with this event
        /// </summary>
        [DataMember(Name = "meet_url")]
        public string MeetUrl { get; set; }
        /// <summary>
        /// Age range of event
        /// </summary>
        [DataMember(Name = "age_range")]
        public string AgeRange { get; set; }
        /// <summary>
        /// Gender catagory
        /// </summary>
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        /// <summary>
        /// Distance of swim (m)
        /// </summary>
        [DataMember(Name = "distace_in_meters")]
        public int? DistanceInMeters { get; set; }
        /// <summary>
        /// Number of lanes
        /// </summary>
        [DataMember(Name = "lanes")]
        public int? Lanes { get; set; }
        /// <summary>
        /// Stroke type
        /// </summary>
        [DataMember(Name = "stroke")]
        public string Stroke { get; set; }
        /// <summary>
        /// Round in series of events
        /// </summary>
        [DataMember(Name = "round")]
        public string Round { get; set; }
        /// <summary>
        /// Date and time of event
        /// </summary>
        [DataMember(Name = "date_and_time")]
        public DateTime? EventDateTime { get; set; }

        /// <summary>
        /// Competitors enlisted on event
        /// </summary>
        [DataMember(Name = "competitors")]
        public List<CompetitorViewModel> Competitors { get; set; }
    }

    /// <summary>
    /// Edit the details of an event
    /// </summary>
    [DataContract(Name = "event_edit")]
    public class EventEditViewModel
    {
        /// <summary>
        /// Amend the date and time
        /// </summary>
        [DataMember(Name = "date_and_time")]
        public DateTime? EventDateTime { get; set; }
    }
}