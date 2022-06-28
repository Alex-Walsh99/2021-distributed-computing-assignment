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
using Sem_2_Swimclub.Models;
using Sem_2_Swimclub.Models.ViewModels;

namespace Sem_2_Swimclub.Controllers
{
    public class EventsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a historical list of all the events on the system.
        /// </summary>
        /// <returns></returns>
        // GET: api/Events
        public List<EventViewModel> GetEvents()
        {
            List<EventViewModel> events = new List<EventViewModel>();
            foreach (Event @event in db.Events.ToList())
            {
                events.Add(
                    new EventViewModel
                    {
                        MeetUrl = Url.Link("DefaultApi", new { controller = "Meets", id = @event.MeetId }),
                        AgeRange = @event.AgeRange,
                        Gender = @event.Gender,
                        DistanceInMeters = @event.DistanceinMeters,
                        Lanes = @event.Lanes,
                        Stroke = @event.Stroke,
                        Round = @event.Round,
                        Competitors = GetCompetitorsFor(@event)
                    }
                );
            }
            return events;
        }

        /// <summary>
        /// Returns the event at a specific ID.
        /// </summary>
        /// <param name="id">The ID of the event to find.</param>
        /// <returns></returns>
        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            EventViewModel eventView = new EventViewModel
            {
                MeetUrl = Url.Link("DefaultApi", new { controller = "Meets", id = @event.MeetId}),
                AgeRange = @event.AgeRange,
                Gender = @event.Gender,
                DistanceInMeters = @event.DistanceinMeters,
                Lanes = @event.Lanes,
                Stroke = @event.Stroke,
                Round = @event.Round,
                EventDateTime = @event.EventDateTime,
                Competitors = GetCompetitorsFor(@event)
            };
            return Ok(eventView);
        }

        private List<CompetitorViewModel> GetCompetitorsFor(Event eventModel)
        {
            List<CompetitorViewModel> competitors = new List<CompetitorViewModel>();
            foreach (Competitor i_competitor in eventModel.Competitors.ToList())
            {
                competitors.Add(
                    new CompetitorViewModel
                    {
                        EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = i_competitor.EventId }),
                        SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = i_competitor.UserId }),
                        Lane = i_competitor.Lane,
                        TimeInSeconds = (double)i_competitor.TimeInSeconds.GetValueOrDefault(),
                        ReasonNotFinished = i_competitor.ReasonNotFinished
                    }
                );
            }
            return competitors;
        }

        /// <summary>
        /// Change the details of a specific Event.
        /// </summary>
        /// <param name="id">The id of the event to update.</param>
        /// <param name="editModel">The details to change.</param>
        /// <returns></returns>
        [Authorize(Roles = "Club Member")]
        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEvent(int id, EventEditViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event @event = db.Events.Find(id);

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            @event.EventDateTime = editModel.EventDateTime;
            db.Entry(@event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a new event to the system.
        /// </summary>
        /// <param name="event">The details of the new event to add.</param>
        /// <returns></returns>
        [Authorize(Roles = "Club Member")]
        // POST: api/Events
        [ResponseType(typeof(Event))]
        public IHttpActionResult PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @event.EventId }, @event);
        }
        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = "Club Member")]
        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventId == id) > 0;
        }
    }
}