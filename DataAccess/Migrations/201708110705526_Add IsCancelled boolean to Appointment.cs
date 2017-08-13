namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsCancelledbooleantoAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "IsCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "IsCancelled");
        }
    }
}
