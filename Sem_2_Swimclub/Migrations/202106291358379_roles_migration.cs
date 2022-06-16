namespace Sem_2_Swimclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles_migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "Role_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUserRoles", "Role_Id");
            CreateIndex("dbo.AspNetUserRoles", "User_Id");
            AddForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserRoles", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropColumn("dbo.AspNetUserRoles", "User_Id");
            DropColumn("dbo.AspNetUserRoles", "Role_Id");
            DropColumn("dbo.AspNetUserRoles", "Discriminator");
        }
    }
}
