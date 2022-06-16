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
    public class FamilyGroupsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: api/FamilyGroups
        [Authorize(Roles = "Club Member")]
        [Route("api/FamilyGroups/")]
        public List<FamilyGroupViewModel> GetFamilyGroups()
        {
            List<FamilyGroupViewModel> familyGroups = new List<FamilyGroupViewModel>();

            foreach (FamilyGroup group in db.FamilyGroup.ToList())
            {
                familyGroups.Add(
                    new FamilyGroupViewModel
                    {
                        ParentUrl = Url.Link("DefaultApi", new { controller = "Account", userId = group.ParentID }),
                        PhoneNumber = group.PhoneNumber,
                        Email = group.Email,
                        AddressLine1 = group.AddressLine1,
                        AddressLine2 = group.AddressLine2,
                        City = group.City,
                        PostCode = group.Postcode,
                        Children = GetChildRelFor(group)
                    }
                    );
            }
            return familyGroups;
        }

        // GET: api/FamilyGroups/UserDetails
        [Authorize]
        [Route("api/FamilyGroup/")]
        [ResponseType(typeof(FamilyGroup))]
        [Route("api/FamilyGroups/UserDetails", Name = "GetFamilyGroupUserDetails")]
        public IHttpActionResult GetFamilyGroupUserDetails()
        {
            FamilyGroup group = db.FamilyGroup.Find(User.Identity.GetUserId());
            if (group == null)
            {
                return NotFound();
            }

            return Ok(new FamilyGroupViewModel
            {
                ParentUrl = Url.Link("DefaultApi", new { controller = "Account", userId = group.ParentID }),
                PhoneNumber = group.PhoneNumber,
                Email = group.Email,
                AddressLine1 = group.AddressLine1,
                AddressLine2 = group.AddressLine2,
                City = group.City,
                PostCode = group.Postcode,
                Children = GetChildRelFor(group)
            });
        }

        // GET: api/FamilyGroups/5
        [ResponseType(typeof(FamilyGroup))]
        [Route("api/FamilyGroups/{id}", Name = "GetFamilyGroupById")]
        public IHttpActionResult GetFamilyGroup(string id)
        {
            FamilyGroup group = db.FamilyGroup.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(new FamilyGroupViewModel
            {
                ParentUrl = Url.Link("DefaultApi", new { controller = "Account", userId = group.ParentID }),
                PhoneNumber = group.PhoneNumber,
                Email = group.Email,
                AddressLine1 = group.AddressLine1,
                AddressLine2 = group.AddressLine2,
                City = group.City,
                PostCode = group.Postcode,
                Children = GetChildRelFor(group)
            });
        }

        private List<ChildViewModel> GetChildRelFor(FamilyGroup group) {
            List<ChildViewModel> childrenRel = new List<ChildViewModel>();
            foreach (ChildFamilyGroupRel childRel in group.ChildFamilyGroupRel.ToList())
            {
                childrenRel.Add(
                    new ChildViewModel
                    {
                        FamilyGroupUrl = Url.Link("DefaultApi", new { controller = "FamilyGroups", id = childRel.FamilyGroupID}),
                        ChildUserUrl = Url.Link("DefaultApi", new { controller = "ChildFamilyGroupRels", id = childRel.UserID})
                    }
                );
            }
            return childrenRel;
        }
        [Authorize(Roles = "Club Member")]
        // PUT: api/FamilyGroups/5
        [Route("api/FamilyGroups/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFamilyGroup(string id, FamilyGroup familyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != familyGroup.ParentID)
            {
                return BadRequest();
            }

            db.Entry(familyGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyGroupExists(id))
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

        [Authorize(Roles = "Parent")]
        // PUT: api/FamilyGroups/
        [Route("api/FamilyGroup")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult EditContactNumber(FamilyGroupEditViewModel editModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string id = User.Identity.GetUserId();
            FamilyGroup familyGroup = db.FamilyGroup.Find(id);

            if (id != familyGroup.ParentID)
            {
                return BadRequest();
            }

            familyGroup.PhoneNumber = editModel.PhoneNumber;

            db.Entry(familyGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyGroupExists(id))
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

        [Authorize(Roles = "Club Member")]
        // POST: api/FamilyGroups
        [Route("api/FamilyGroups/")]
        [ResponseType(typeof(FamilyGroup))]
        public IHttpActionResult PostFamilyGroup(FamilyGroup familyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FamilyGroup.Add(familyGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FamilyGroupExists(familyGroup.ParentID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetFamilyGroupUserDetails", new { }, familyGroup);
        }
        [Authorize(Roles = "Club Member")]
        // POST: api/FamilyGroups/ChildGroupRel
        [Route("api/FamilyGroups/ChildGroupRel")]
        [ResponseType(typeof(ChildFamilyGroupRel))]
        public IHttpActionResult PostChildGroupRel(ChildFamilyGroupRel childGroupRel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ChildFamilyGroupRel.Add(childGroupRel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ChildGroupRelExists(childGroupRel.FamilyGroupID, childGroupRel.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetFamilyGroupUserDetails", new {}, childGroupRel.FamilyGroup);
        }

        private bool ChildGroupRelExists(string familyGroupID, string userID)
        {
            return db.ChildFamilyGroupRel.Count(e => e.FamilyGroupID == familyGroupID && e.UserID == userID) > 0;
        }
        [Authorize(Roles = "Club Member")]
        // DELETE: api/FamilyGroups/5
        [Route("api/FamilyGroups/{id}")]
        [ResponseType(typeof(FamilyGroup))]
        public IHttpActionResult DeleteFamilyGroup(string id)
        {
            FamilyGroup familyGroup = db.FamilyGroup.Find(id);
            if (familyGroup == null)
            {
                return NotFound();
            }

            db.FamilyGroup.Remove(familyGroup);
            db.SaveChanges();

            return Ok(familyGroup);
        }

        [Authorize(Roles = "Club Member")]
        // DELETE: api/FamilyGroups/5,302c1b8d-3036-40af-8a9c-f0d5dc8c1572
        [Route("api/FamilyGroups/")]
        [ResponseType(typeof(ChildFamilyGroupRel))]
        public IHttpActionResult DeleteChildGroupRel(string familyGroupId, string userId)
        {
            ChildFamilyGroupRel childGroupRel = db.ChildFamilyGroupRel.Find(familyGroupId, userId);
            if (childGroupRel == null)
            {
                return NotFound();
            }

            db.ChildFamilyGroupRel.Remove(childGroupRel);
            db.SaveChanges();

            return Ok(childGroupRel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FamilyGroupExists(string id)
        {
            return db.FamilyGroup.Count(e => e.ParentID == id) > 0;
        }
    }
}