namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassRooms", "FaculteId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "SpecialiteId", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "AnneeId", c => c.Int(nullable: false));
            AddColumn("dbo.Groupes", "Semestre", c => c.Int(nullable: false));
            CreateIndex("dbo.ClassRooms", "FaculteId");
            CreateIndex("dbo.Courses", "SpecialiteId");
            CreateIndex("dbo.Courses", "AnneeId");
            AddForeignKey("dbo.ClassRooms", "FaculteId", "dbo.Facultes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Courses", "AnneeId", "dbo.Annees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Courses", "SpecialiteId", "dbo.Specialites", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "SpecialiteId", "dbo.Specialites");
            DropForeignKey("dbo.Courses", "AnneeId", "dbo.Annees");
            DropForeignKey("dbo.ClassRooms", "FaculteId", "dbo.Facultes");
            DropIndex("dbo.Courses", new[] { "AnneeId" });
            DropIndex("dbo.Courses", new[] { "SpecialiteId" });
            DropIndex("dbo.ClassRooms", new[] { "FaculteId" });
            DropColumn("dbo.Groupes", "Semestre");
            DropColumn("dbo.Courses", "AnneeId");
            DropColumn("dbo.Courses", "SpecialiteId");
            DropColumn("dbo.ClassRooms", "FaculteId");
        }
    }
}
