namespace TechZone.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusForOrdrerDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Status");
        }
    }
}
