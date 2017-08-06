namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAppointmentsActivitiesconnectionmanytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Activities", new[] { "AppointmentId" });
            CreateTable(
                "dbo.AppointmentActivities",
                c => new
                    {
                        Appointment_AppointmentId = c.Int(nullable: false),
                        Activity_ActivityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Appointment_AppointmentId, t.Activity_ActivityId })
                .ForeignKey("dbo.Appointments", t => t.Appointment_AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.Activity_ActivityId, cascadeDelete: true)
                .Index(t => t.Appointment_AppointmentId)
                .Index(t => t.Activity_ActivityId);
            
            DropColumn("dbo.Activities", "AppointmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "AppointmentId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AppointmentActivities", "Activity_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.AppointmentActivities", "Appointment_AppointmentId", "dbo.Appointments");
            DropIndex("dbo.AppointmentActivities", new[] { "Activity_ActivityId" });
            DropIndex("dbo.AppointmentActivities", new[] { "Appointment_AppointmentId" });
            DropTable("dbo.AppointmentActivities");
            CreateIndex("dbo.Activities", "AppointmentId");
            AddForeignKey("dbo.Activities", "AppointmentId", "dbo.Appointments", "AppointmentId", cascadeDelete: true);
        }
    }
}
