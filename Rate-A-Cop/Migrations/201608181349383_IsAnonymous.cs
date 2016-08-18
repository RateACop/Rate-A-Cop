namespace Rate_A_Cop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsAnonymous : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "IsAnonymous", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "IsAnonymous");
        }
    }
}
