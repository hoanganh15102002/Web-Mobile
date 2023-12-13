namespace Mobile_ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProDes = c.String(),
                        Images = c.String(),
                        ImageNote = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        InfoDes = c.String(),
                        CategoryID = c.Int(nullable: false),
                        OrderDetail_OrderDetailID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetail_OrderDetailID)
                .Index(t => t.CategoryID)
                .Index(t => t.OrderDetail_OrderDetailID);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        TotalMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerID = c.Int(nullable: false),
                        ShippingID = c.Int(nullable: false),
                        OrderStatusID = c.Int(nullable: false),
                        VoucherID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Vouchers", t => t.VoucherID)
                .Index(t => t.CustomerID)
                .Index(t => t.VoucherID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false),
                        Image = c.String(),
                        CustomerName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Users", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserPass = c.String(),
                        Status = c.Boolean(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Des = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        OrderStatusID = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Order_OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderStatusID)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .Index(t => t.Order_OrderID);
            
            CreateTable(
                "dbo.Shippings",
                c => new
                    {
                        ShippingID = c.Int(nullable: false, identity: true),
                        ShippingMethod = c.String(),
                        PriceShipping = c.Int(nullable: false),
                        Order_OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.ShippingID)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .Index(t => t.Order_OrderID);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        VoucherID = c.String(nullable: false, maxLength: 128),
                        VoucherName = c.String(),
                        VouDes = c.String(),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VoucherID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        tenkh = c.String(nullable: false),
                        mail = c.String(nullable: false),
                        dienthoai = c.String(nullable: false),
                        tieude = c.String(nullable: false),
                        noidung = c.String(),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.ReviewProes",
                c => new
                    {
                        ReviewProID = c.Int(nullable: false, identity: true),
                        RateValue = c.Int(nullable: false),
                        Comment = c.String(),
                        ReviewerName = c.String(),
                        ReviewContent = c.String(),
                        ReviewDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewProID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "OrderDetail_OrderDetailID", "dbo.OrderDetails");
            DropForeignKey("dbo.Orders", "VoucherID", "dbo.Vouchers");
            DropForeignKey("dbo.Shippings", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderStatus", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Customers", "CustomerID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Shippings", new[] { "Order_OrderID" });
            DropIndex("dbo.OrderStatus", new[] { "Order_OrderID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Customers", new[] { "CustomerID" });
            DropIndex("dbo.Orders", new[] { "VoucherID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Products", new[] { "OrderDetail_OrderDetailID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.ReviewProes");
            DropTable("dbo.Contacts");
            DropTable("dbo.Vouchers");
            DropTable("dbo.Shippings");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
