namespace Sem_2_Swimclub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Competitors", "Lane", c => c.Int());
            AlterColumn("dbo.Events", "DistanceinMeters", c => c.Int());
            AlterColumn("dbo.Events", "Lanes", c => c.Int());
            AlterColumn("dbo.Events", "EventDateTime", c => c.DateTime());
            AlterColumn("dbo.Meets", "MeetDateTime", c => c.DateTime());
            AlterColumn("dbo.Meets", "PoolSizeInMeters", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meets", "PoolSizeInMeters", c => c.Int(nullable: false));
            AlterColumn("dbo.Meets", "MeetDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Events", "EventDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Events", "Lanes", c => c.Int(nullable: false));
            AlterColumn("dbo.Events", "DistanceinMeters", c => c.Int(nullable: false));
            AlterColumn("dbo.Competitors", "Lane", c => c.Int(nullable: false));
        }
    }
}
