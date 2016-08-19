namespace Rate_A_Cop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewReviewDateTimeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ReviewDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "ReviewDateTime");
        }
    }
}
