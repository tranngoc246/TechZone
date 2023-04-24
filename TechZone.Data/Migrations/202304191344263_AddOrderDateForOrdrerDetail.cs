namespace TechZone.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderDateForOrdrerDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "OrderDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "OrderDate");
        }
    }
}
