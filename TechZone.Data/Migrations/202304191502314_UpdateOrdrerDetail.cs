namespace TechZone.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrdrerDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "DeliveryDate", c => c.DateTime());
            AddColumn("dbo.OrderDetails", "IsOrder", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetails", "IsDelivery", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrderDetails", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrderDetails", "IsDelivery");
            DropColumn("dbo.OrderDetails", "IsOrder");
            DropColumn("dbo.OrderDetails", "DeliveryDate");
        }
    }
}
