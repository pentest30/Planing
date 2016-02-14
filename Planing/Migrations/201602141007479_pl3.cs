namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Type", c => c.String(maxLength: 4000));
        }
    }
}
