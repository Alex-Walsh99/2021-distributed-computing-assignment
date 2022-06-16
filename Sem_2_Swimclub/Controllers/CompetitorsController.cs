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
using Sem_2_Swimclub.Models.ViewModels;

namespace Sem_2_Swimclub.Controllers
{
    public class CompetitorsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Competitors
        public List<CompetitorViewModel> GetCompetitors()
        {
            List<CompetitorViewModel> competitors = new List<CompetitorViewModel>();
            foreach(Competitor competitor in db.Competitors.ToList())
            {
                competitors.Add
                    (
                        new CompetitorViewModel
                        {
                            EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = competitor.EventId }),
                            SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = competitor.UserId }),
                            Lane = competitor.Lane,
                            TimeInSeconds = (double) competitor.TimeInSeconds.GetValueOrDefault(),
                            ReasonNotFinished = competitor.ReasonNotFinished
                        }
                    );
            }
            return competitors;
        }


        [AllowAnonymous]
        public List<CompetitorViewModel> GetCompetitors(string lastName, int? minAge, int? maxAge, string stroke)
        {
            List<CompetitorViewModel> competitorsModel = new List<CompetitorViewModel>();

            var competitiors = db.Competitors.Include(c => c.Event).Include(c => c.User);

            if (minAge != null)
                competitiors = competitiors.Where(c => c.User.DateOfBirth.Year < (DateTime.Now.Year - minAge));
            if (maxAge != null)
                competitiors = competitiors.Where(c => c.User.DateOfBirth.Year > (DateTime.Now.Year - maxAge));

            if (!String.IsNullOrEmpty(lastName))
                competitiors = competitiors.Where(c => c.User.LastName.Contains(lastName));
            if (!String.IsNullOrEmpty(stroke))
                competitiors = competitiors.Where(c => c.Event.Stroke.Contains(stroke));

            foreach (Competitor competitor in competitiors.ToList())
            {
                competitorsModel.Add(
                    new CompetitorViewModel
                    {
                        EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = competitor.EventId }),
                        SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = competitor.UserId }),
                        Lane = competitor.Lane,
                        TimeInSeconds = (double)competitor.TimeInSeconds.GetValueOrDefault(),
                        ReasonNotFinished = competitor.ReasonNotFinished
                    }
                );
            }
            return competitorsModel;
        }

        // GET: api/Competitors/5
        [ResponseType(typeof(Competitor))]
        public IHttpActionResult GetCompetitor(int id)
        {
            Competitor competitor = db.Competitors.Find(id);
            if (competitor == null)
            {
                return NotFound();
            }
            CompetitorViewModel competitorView = new CompetitorViewModel
            {
                EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = competitor.EventId }),
                SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = competitor.UserId }),
                Lane = competitor.Lane,
                TimeInSeconds = (double) competitor.TimeInSeconds,
                ReasonNotFinished = competitor.ReasonNotFinished
            };
            return Ok(competitorView);
        }

        // GET: api/Competitors/5
        [Authorize(Roles = "Swimmer")]
        [ResponseType(typeof(Competitor))]
        [Route("api/Competitors/Competitons")]
        public IHttpActionResult GetUserCompetitions()
        {
            List<UserCompetitionViewModel> competitors = new List<UserCompetitionViewModel>();
            string userid = User.Identity.GetUserId();
            var competitions = from c in db.Competitors
                              where c.UserId == userid
                               select c;
            foreach (Competitor competitor in competitions.ToList())
            {
                competitors.Add
                    (
                        new UserCompetitionViewModel
                        {
                            EventUrl = Url.Link("DefaultApi", new { controller = "Events", id = competitor.EventId }),
                            SwimmerUrl = Url.Link("DefaultApi", new { controller = "Account", userId = competitor.UserId }),
                            MeetUrl = Url.Link("DefaultApi", new { controller = "Meet", id = competitor.Event.MeetId}),
                            Lane = competitor.Lane,
                            TimeInSeconds = (double)competitor.TimeInSeconds,
                            ReasonNotFinished = competitor.ReasonNotFinished
                        }
                    );
            }
            return Ok(competitors);
        }


        // PUT: api/Competitors/5
        [Authorize(Roles = "Club Member")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompetitor(int id, Competitor competitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != competitor.EventId)
            {
                return BadRequest();
            }

            db.Entry(competitor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitorExists(id))
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
       
        // POST: api/Competitors
        [ResponseType(typeof(Competitor))] 
        [Authorize(Roles = "Club Member")]
        public IHttpActionResult PostCompetitor(Competitor competitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Competitors.Add(competitor);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CompetitorExists(competitor.EventId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = competitor.EventId }, competitor);
        }
       
        // DELETE: api/Competitors/5
        [ResponseType(typeof(Competitor))] 
        [Authorize(Roles = "Club Member")]
        public IHttpActionResult DeleteCompetitor(int id)
        {
            Competitor competitor = db.Competitors.Find(id);
            if (competitor == null)
            {
                return NotFound();
            }

            db.Competitors.Remove(competitor);
            db.SaveChanges();

            return Ok(competitor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompetitorExists(int id)
        {
            return db.Competitors.Count(e => e.EventId == id) > 0;
        }
    }
}