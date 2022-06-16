namespace Sem_2_Swimclub.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Sem_2_Swimclub.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sem_2_Swimclub.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sem_2_Swimclub.Models.ApplicationDbContext context)
        {
            //// Roles
            //List<IdentityRole> Roles = new List<IdentityRole>
            //{
            //    new IdentityRole {Name = "Club Member"},
            //    new IdentityRole {Name = "Parent"},
            //    new IdentityRole {Name = "Swimmer"},
            //};
            //Roles.ForEach(r => context.Roles.AddOrUpdate(r_in_list => r_in_list.Name, r));
            //context.SaveChanges();
            //// Users
            //context.Users.AddOrUpdate(
            //    new ApplicationUser
            //    {
            //        Title = "Mr",
            //        FirstName = "Alex",
            //        LastName = "Walsh",
            //        Inactive = false,
            //        Email = "alex.walsh@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "alex.walsh@stokeswimclub.com",
            //        DateOfBirth = new DateTime(1999, 04, 29),
            //        Gender = "Male"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Mr",
            //        FirstName = "John",
            //        LastName = "Doe",
            //        Inactive = false,
            //        Email = "john.doe@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "john.doe@stokeswimclub.com",
            //        DateOfBirth = new DateTime(1971, 06, 21),
            //        Gender = "Male"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Mrs",
            //        FirstName = "Stacy",
            //        LastName = "Smith",
            //        Inactive = false,
            //        Email = "stacy.smith@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "stacy.smith@stokeswimclub.com",
            //        DateOfBirth = new DateTime(1975, 03, 03),
            //        Gender = "Male"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Mr",
            //        FirstName = "Billy",
            //        LastName = "Smith",
            //        Inactive = false,
            //        Email = "billy.smith@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "billy.smith@stokeswimclub.com",
            //        DateOfBirth = new DateTime(2011, 06, 23),
            //        Gender = "Male"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Miss",
            //        FirstName = "Sarah",
            //        LastName = "Smith",
            //        Inactive = false,
            //        Email = "sarah.smith@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "sarah.smith@stokeswimclub.com",
            //        DateOfBirth = new DateTime(2009, 03, 12),
            //        Gender = "Female"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Mr",
            //        FirstName = "Jimmy",
            //        LastName = "Doe",
            //        Inactive = false,
            //        Email = "jimmy.doe@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "jimmy.doe@stokeswimclub.com",
            //        DateOfBirth = new DateTime(2013, 02, 12),
            //        Gender = "Male"
            //    },
            //    new ApplicationUser
            //    {
            //        Title = "Miss",
            //        FirstName = "Elizabeth",
            //        LastName = "Smith",
            //        Inactive = false,
            //        Email = "elizabeth.doe@stokeswimclub.com",
            //        EmailConfirmed = true,
            //        PhoneNumberConfirmed = false,
            //        TwoFactorEnabled = false,
            //        LockoutEnabled = false,
            //        UserName = "elizabeth.doe@stokeswimclub.com",
            //        DateOfBirth = new DateTime(2014, 07, 20),
            //        Gender = "Female"
            //    });
            //context.SaveChanges();
        }
    }
}
