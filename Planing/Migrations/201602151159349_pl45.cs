namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Specialites", "Code", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Specialites", "Code");
        }
    }
}
