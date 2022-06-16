namespace Sem_2_Swimclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChildFamilyGroupRels",
                c => new
                    {
                        FamilyGroupID = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FamilyGroupID, t.UserID })
                .ForeignKey("dbo.FamilyGroups", t => t.FamilyGroupID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.FamilyGroupID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.FamilyGroups",
                c => new
                    {
                        ParentID = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(),
                        Email = c.String(maxLength: 256),
                        AddressLine1 = c.String(maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Postcode = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ParentID)
                .ForeignKey("dbo.AspNetUsers", t => t.ParentID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(maxLength: 5),
                        FirstName = c.String(maxLength: 20),
                        LastName = c.String(maxLength: 20),
                        Inactive = c.Boolean(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Competitors",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Lane = c.Int(nullable: false),
                        TimeInSeconds = c.Double(),
                        ReasonNotFinished = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        MeetId = c.Int(nullable: false),
                        AgeRange = c.String(maxLength: 50),
                        Gender = c.String(maxLength: 50),
                        DistanceinMeters = c.Int(nullable: false),
                        Lanes = c.Int(nullable: false),
                        Stroke = c.String(maxLength: 50),
                        Round = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Meets", t => t.MeetId, cascadeDelete: true)
                .Index(t => t.MeetId);
            
            CreateTable(
                "dbo.Meets",
                c => new
                    {
                        MeetId = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Postcode = c.String(maxLength: 10),
                        MeetDateTime = c.DateTime(nullable: false),
                        PoolSizeInMeters = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MeetId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ChildFamilyGroupRels", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChildFamilyGroupRels", "FamilyGroupID", "dbo.FamilyGroups");
            DropForeignKey("dbo.FamilyGroups", "ParentID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Competitors", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Competitors", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "MeetId", "dbo.Meets");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Events", new[] { "MeetId" });
            DropIndex("dbo.Competitors", new[] { "UserId" });
            DropIndex("dbo.Competitors", new[] { "EventId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.FamilyGroups", new[] { "ParentID" });
            DropIndex("dbo.ChildFamilyGroupRels", new[] { "UserID" });
            DropIndex("dbo.ChildFamilyGroupRels", new[] { "FamilyGroupID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Meets");
            DropTable("dbo.Events");
            DropTable("dbo.Competitors");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FamilyGroups");
            DropTable("dbo.ChildFamilyGroupRels");
        }
    }
}
