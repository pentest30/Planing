namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl311 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "Numero", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "Numero");
        }
    }
}
