namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Specialites", "AnneeId", "dbo.Annees");
            DropIndex("dbo.Specialites", new[] { "AnneeId" });
            DropColumn("dbo.Specialites", "AnneeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Specialites", "AnneeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Specialites", "AnneeId");
            AddForeignKey("dbo.Specialites", "AnneeId", "dbo.Annees", "Id", cascadeDelete: true);
        }
    }
}
