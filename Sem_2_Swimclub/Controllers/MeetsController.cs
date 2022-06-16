using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Sem_2_Swimclub.Models;
using Sem_2_Swimclub.Models.BindingModels;
using Sem_2_Swimclub.Models.ViewModels;

namespace Sem_2_Swimclub.Controllers
{
    public class MeetsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Meets
        public List<MeetViewModel> GetMeets()
        {
            List<MeetViewModel> meets = new List<MeetViewModel>();

            foreach(Meet meet in db.Meets.ToList())
            {
                meets.Add(
                    new MeetViewModel
                        {
                            AddressLine1 = meet.AddressLine1,
                            AddressLine2 = meet.AddressLine2,
                            City = meet.City,
                            Postcode = meet.Postcode,
                            MeetDateTime = meet.MeetDateTime,
                            PoolSizeInMeters = meet.PoolSizeInMeters,
                            Events = GetEventsFor(meet)
                        }
                    );
            }
            return meets;
        }

        private List<EventViewModel> GetEventsFor(Meet meet)
        {
            List<EventViewModel> events = new List<EventViewModel>();
            foreach(Event i_event in meet.Events.ToList())
            {
                events.Add(
                    new EventViewModel
                    {
                        MeetUrl = Url.Link("DefaultApi", new { controller = "Meets", id = i_event.MeetId}),
                        AgeRange = i_event.AgeRange,
                        Gender = i_event.Gender,
                        DistanceInMeters = i_event.DistanceinMeters,
                        Lanes = i_event.Lanes,
                        Stroke = i_event.Stroke,
                        Round = i_event.Round,
                        Competitors = GetCompetitorsFor(i_event)

                    }
                );
            }
            return events;
        }

        private List<CompetitorViewModel> GetCompetitorsFor(Event eventModel)
        {
            List<CompetitorViewModel> competitors = new List<CompetitorViewModel>();
            foreach (Competitor i_competitor in eventModel.Competitors.ToList())
            {

                //crashes because of null value
                competitors.Add(
                    new CompetitorViewModel
                    {
                        EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = i_competitor.EventId }),
                        SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = i_competitor.UserId }),
                        Lane = i_competitor.Lane,
                        TimeInSeconds = (double) i_competitor.TimeInSeconds.GetValueOrDefault(),
                        ReasonNotFinished = i_competitor.ReasonNotFinished
                    }
                );
            }
            return competitors;
        }

        // GET: api/Meets/5
        [ResponseType(typeof(Meet))]
        public IHttpActionResult GetMeet(int id)
        {
            Meet meet = db.Meets.Find(id);
            if (meet == null)
            {
                return NotFound();
            }

            MeetViewModel meetView = new MeetViewModel
            {
                AddressLine1 = meet.AddressLine1,
                AddressLine2 = meet.AddressLine2,
                City = meet.City,
                Postcode = meet.Postcode,
                MeetDateTime = meet.MeetDateTime,
                PoolSizeInMeters = meet.PoolSizeInMeters,
                Events = GetEventsFor(meet)
            };

            return Ok(meetView);
        }

        // PUT: api/Meets/5
        [Authorize(Roles = "Club Member")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeet(int id, MeetBindingModel meetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Meet meet = db.Meets.Find(id);
            if (meet != null)
            {
                meet.AddressLine1 = meetModel.AddressLine1;
                meet.AddressLine2 = meetModel.AddressLine2;
                meet.City = meetModel.City;
                meet.Postcode = meetModel.Postcode;
                meet.MeetDateTime = meetModel.MeetDateTime;
                meet.PoolSizeInMeters = meetModel.PoolSizeInMeters;
            }
            else
                return NotFound(); 

            db.Entry(meet).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Club Member")]
        // POST: api/Meets
        [ResponseType(typeof(Meet))]
        public IHttpActionResult PostMeet(Meet meet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Meets.Add(meet);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = meet.MeetId }, meet);
        }
        [Authorize(Roles = "Club Member")]
        // DELETE: api/Meets/5
        [ResponseType(typeof(Meet))]
        public IHttpActionResult DeleteMeet(int id)
        {
            Meet meet = db.Meets.Find(id);
            if (meet == null)
            {
                return NotFound();
            }

            db.Meets.Remove(meet);
            db.SaveChanges();

            return Ok(meet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeetExists(int id)
        {
            return db.Meets.Count(e => e.MeetId == id) > 0;
        }
    }
}