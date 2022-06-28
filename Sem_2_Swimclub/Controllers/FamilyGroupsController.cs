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
        
        /// <summary>
        /// Returns all family groups in the system.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the family group details of the user currently connected.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the details of a family group with a specific ID.
        /// </summary>
        /// <param name="id">The ID of the group to be queried</param>
        /// <returns></returns>
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
        /// <summary>
        /// Amends the details of a family group as an administrator.
        /// </summary>
        /// <param name="id">The ID of the group to be updated.</param>
        /// <param name="familyGroup">A list of the information to change.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Amend the details of your family group as a parent.
        /// </summary>
        /// <param name="editModel">A list of the details to edit.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Create a new family group.
        /// </summary>
        /// <param name="familyGroup">The details of the new family group.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Link a child account to a family group.
        /// </summary>
        /// <param name="childGroupRel">Details on the child account to be linked to the family group.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete an existing family group.
        /// </summary>
        /// <param name="id">The ID of the group to be deleted.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove a child account from a family group.
        /// </summary>
        /// <param name="familyGroupId">The family group ID to unlink</param>
        /// <param name="userId">The user to remove from the family group.</param>
        /// <returns></returns>
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