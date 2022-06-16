namespace Sem_2_Swimclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles_migration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "User_Id" });
            DropColumn("dbo.AspNetUserRoles", "Discriminator");
            DropColumn("dbo.AspNetUserRoles", "Role_Id");
            DropColumn("dbo.AspNetUserRoles", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUserRoles", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "Role_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetUserRoles", "User_Id");
            CreateIndex("dbo.AspNetUserRoles", "Role_Id");
            AddForeignKey("dbo.AspNetUserRoles", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles", "Id");
        }
    }
}
