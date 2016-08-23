namespace Rate_A_Cop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latitudelongitude : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Reviews", "Lattitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Lattitude");
            DropColumn("dbo.Reviews", "Longitude");
        }
    }
}
