namespace Sem_2_Swimclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateevent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventDateTime");
        }
    }
}
