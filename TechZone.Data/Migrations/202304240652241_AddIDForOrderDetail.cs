namespace TechZone.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIDForOrderDetail : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrderDetails");
            AddColumn("dbo.OrderDetails", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.OrderDetails", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrderDetails");
            DropColumn("dbo.OrderDetails", "ID");
            AddPrimaryKey("dbo.OrderDetails", new[] { "OrderID", "ProductID" });
        }
    }
}
