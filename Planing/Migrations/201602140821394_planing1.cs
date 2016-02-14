namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class planing1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Annees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnneeScolaires",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Code = c.String(maxLength: 4000),
                        Type = c.String(maxLength: 4000),
                        MinSize = c.Int(nullable: false),
                        MaxSize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassRoomTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Code = c.String(maxLength: 4000),
                        Type = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Facultes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groupes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        SectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        AnneeId = c.Int(nullable: false),
                        SpecialiteId = c.Int(nullable: false),
                        AnneeScolaireId = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Annees", t => t.AnneeId, cascadeDelete: true)
                .ForeignKey("dbo.AnneeScolaires", t => t.AnneeScolaireId, cascadeDelete: true)
                .ForeignKey("dbo.Specialites", t => t.SpecialiteId, cascadeDelete: true)
                .Index(t => t.AnneeId)
                .Index(t => t.SpecialiteId)
                .Index(t => t.AnneeScolaireId);
            
            CreateTable(
                "dbo.Specialites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        FaculteId = c.Int(nullable: false),
                        AnneeId = c.Int(nullable: false),
                        NiveauId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Annees", t => t.AnneeId, cascadeDelete: false)
                .ForeignKey("dbo.Facultes", t => t.FaculteId, cascadeDelete: true)
                .ForeignKey("dbo.Niveaux", t => t.NiveauId, cascadeDelete: true)
                .Index(t => t.FaculteId)
                .Index(t => t.AnneeId)
                .Index(t => t.NiveauId);
            
            CreateTable(
                "dbo.Niveaux",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnneScolaireId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Day = c.String(maxLength: 4000),
                        HourStart = c.String(maxLength: 4000),
                        HourEnd = c.String(maxLength: 4000),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tcs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        ScheduleWieght = c.Int(nullable: false),
                        AnneeScolaireId = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        SectionId = c.Int(),
                        GroupeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnneeScolaires", t => t.AnneeScolaireId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Groupes", t => t.GroupeId)
                .ForeignKey("dbo.Sections", t => t.SectionId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.CourseId)
                .Index(t => t.AnneeScolaireId)
                .Index(t => t.SectionId)
                .Index(t => t.GroupeId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomComplet = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tcs", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Tcs", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Tcs", "GroupeId", "dbo.Groupes");
            DropForeignKey("dbo.Tcs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Tcs", "AnneeScolaireId", "dbo.AnneeScolaires");
            DropForeignKey("dbo.Groupes", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "SpecialiteId", "dbo.Specialites");
            DropForeignKey("dbo.Specialites", "NiveauId", "dbo.Niveaux");
            DropForeignKey("dbo.Specialites", "FaculteId", "dbo.Facultes");
            DropForeignKey("dbo.Specialites", "AnneeId", "dbo.Annees");
            DropForeignKey("dbo.Sections", "AnneeScolaireId", "dbo.AnneeScolaires");
            DropForeignKey("dbo.Sections", "AnneeId", "dbo.Annees");
            DropIndex("dbo.Tcs", new[] { "GroupeId" });
            DropIndex("dbo.Tcs", new[] { "SectionId" });
            DropIndex("dbo.Tcs", new[] { "AnneeScolaireId" });
            DropIndex("dbo.Tcs", new[] { "CourseId" });
            DropIndex("dbo.Tcs", new[] { "TeacherId" });
            DropIndex("dbo.Specialites", new[] { "NiveauId" });
            DropIndex("dbo.Specialites", new[] { "AnneeId" });
            DropIndex("dbo.Specialites", new[] { "FaculteId" });
            DropIndex("dbo.Sections", new[] { "AnneeScolaireId" });
            DropIndex("dbo.Sections", new[] { "SpecialiteId" });
            DropIndex("dbo.Sections", new[] { "AnneeId" });
            DropIndex("dbo.Groupes", new[] { "SectionId" });
            DropTable("dbo.Teachers");
            DropTable("dbo.Tcs");
            DropTable("dbo.Seances");
            DropTable("dbo.Niveaux");
            DropTable("dbo.Specialites");
            DropTable("dbo.Sections");
            DropTable("dbo.Groupes");
            DropTable("dbo.Facultes");
            DropTable("dbo.CourseTypes");
            DropTable("dbo.Courses");
            DropTable("dbo.ClassRoomTypes");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.AnneeScolaires");
            DropTable("dbo.Annees");
        }
    }
}
