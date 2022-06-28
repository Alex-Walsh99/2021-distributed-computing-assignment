using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Sem_2_Swimclub.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        //Sets up the fields for each model associated with a table with in the db.
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        
        [StringLength(5)]
        public string Title { get; set; }
        [StringLength(20)]
        public string FirstName { get; set; }
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        public bool Inactive { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public virtual FamilyGroup FamilyGroup { get; set; }
        public virtual List<ChildFamilyGroupRel> Children { get; set; }
        public virtual List<Competitor> Competitions { get; set; }
    }


    //Stores details on groups of family members consisting of a parent/administrator and a list of linked child accounts.
    public class FamilyGroup
    {
        [Key, StringLength(128), ForeignKey("User")]
        public string ParentID { get; set; }
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(50)]
        public string AddressLine1 { get; set; }
        [StringLength(50)]
        public string AddressLine2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(10)]
        public string Postcode { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual List<ChildFamilyGroupRel> ChildFamilyGroupRel { get; set; }
    }

    //A relational table linking a child user to a respective family group
    public class ChildFamilyGroupRel
    {
        [Key, Column(Order = 0), StringLength(128), ForeignKey("FamilyGroup")]
        public string FamilyGroupID { get; set; }
        [Key, Column(Order = 1), StringLength(128), ForeignKey("User")]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual FamilyGroup FamilyGroup { get; set; }
    }

    //Stores information on competitor details such as a link to an event and time in seconds/reason for not finishing
    public class Competitor
    {
        [Key, Column(Order = 0), ForeignKey("Event")]
        public int EventId { get; set; }
        [Key, Column(Order = 1), ForeignKey("User"), StringLength(128)]
        public string UserId { get; set; }
        public int? Lane { get; set; }

        public double? TimeInSeconds { get; set; }
        [StringLength(100)]
        public string ReasonNotFinished { get; set; }

        public virtual Event Event { get; set; }
        public virtual ApplicationUser User { get; set; }

    }

    //Stores details of a particular event as part of a meet.
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [ForeignKey("Meet")]
        public int MeetId { get; set; }
        [StringLength(50)]
        public string AgeRange { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        public int? DistanceinMeters { get; set; }
        public int? Lanes { get; set; }
        [StringLength(50)]
        public string Stroke { get; set; }
        [StringLength(50)]
        public string Round { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? EventDateTime { get; set; }

        public virtual List<Competitor> Competitors { get; set; }

        public virtual Meet Meet { get; set; }

        public string EventName
        {
            get
            {
                return
                    EventId + " - " +
                    AgeRange + " " +
                    Gender + " " +
                    DistanceinMeters.ToString() + " " +
                    Stroke + " " +
                    Round + " " +
                    EventDateTime;
            }
        }

    }

    //Stores details of meets including the meet date/time and address of said meet.
    public class Meet
    {
        [Key]
        public int MeetId { get; set; }
        [StringLength(50)]
        public string AddressLine1 { get; set; }
        [StringLength(50)]
        public string AddressLine2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(10)]
        public string Postcode { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? MeetDateTime { get; set; }
        public int? PoolSizeInMeters { get; set; }

        public virtual List<Event> Events { get; set; }

    }


    //Initializes the collection of models
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<FamilyGroup> FamilyGroup { get; set; }
        public DbSet<ChildFamilyGroupRel> ChildFamilyGroupRel { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Meet> Meets { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}