namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _54 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Seances", "TeacherId");
            AddForeignKey("dbo.Seances", "TeacherId", "dbo.Teachers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Seances", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.Seances", new[] { "TeacherId" });
        }
    }
}
