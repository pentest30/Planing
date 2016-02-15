namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl315 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seances", "Semestre", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Seances", "Semestre");
        }
    }
}
