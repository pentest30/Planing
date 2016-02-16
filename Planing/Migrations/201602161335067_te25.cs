namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class te25 : DbMigration
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
                        FaculteId = c.Int(nullable: false),
                        ClassRoomTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassRoomTypes", t => t.ClassRoomTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Facultes", t => t.FaculteId, cascadeDelete: true)
                .Index(t => t.FaculteId)
                .Index(t => t.ClassRoomTypeId);
            
            CreateTable(
                "dbo.ClassRoomTypes",
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
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Code = c.String(maxLength: 4000),
                        CourseTypeId = c.Int(nullable: false),
                        SpecialiteId = c.Int(nullable: false),
                        AnneeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Annees", t => t.AnneeId, cascadeDelete: true)
                .ForeignKey("dbo.CourseTypes", t => t.CourseTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Specialites", t => t.SpecialiteId, cascadeDelete: true)
                .Index(t => t.CourseTypeId)
                .Index(t => t.SpecialiteId)
                .Index(t => t.AnneeId);
            
            CreateTable(
                "dbo.CourseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specialites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Code = c.String(maxLength: 4000),
                        FilliereId = c.Int(nullable: false),
                        DepartementId = c.Int(nullable: false),
                        FaculteId = c.Int(nullable: false),
                        NiveauId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departements", t => t.DepartementId, cascadeDelete: true)
                .ForeignKey("dbo.Facultes", t => t.FaculteId, cascadeDelete: true)
                .ForeignKey("dbo.Fillieres", t => t.FilliereId, cascadeDelete: true)
                .ForeignKey("dbo.Niveaux", t => t.NiveauId, cascadeDelete: true)
                .Index(t => t.FilliereId)
                .Index(t => t.DepartementId)
                .Index(t => t.FaculteId)
                .Index(t => t.NiveauId);
            
            CreateTable(
                "dbo.Departements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(maxLength: 4000),
                        FaculteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facultes", t => t.FaculteId, cascadeDelete: false)
                .Index(t => t.FaculteId);
            
            CreateTable(
                "dbo.Fillieres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Niveaux",
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
                        Semestre = c.Int(nullable: false),
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
                        Code = c.String(maxLength: 4000),
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
                        Semestre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(maxLength: 4000),
                        Prenom = c.String(maxLength: 4000),
                        Numero = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tcs", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Tcs", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Tcs", "GroupeId", "dbo.Groupes");
            DropForeignKey("dbo.Tcs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Tcs", "AnneeScolaireId", "dbo.AnneeScolaires");
            DropForeignKey("dbo.Seances", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Groupes", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "SpecialiteId", "dbo.Specialites");
            DropForeignKey("dbo.Sections", "AnneeScolaireId", "dbo.AnneeScolaires");
            DropForeignKey("dbo.Sections", "AnneeId", "dbo.Annees");
            DropForeignKey("dbo.Courses", "SpecialiteId", "dbo.Specialites");
            DropForeignKey("dbo.Specialites", "NiveauId", "dbo.Niveaux");
            DropForeignKey("dbo.Specialites", "FilliereId", "dbo.Fillieres");
            DropForeignKey("dbo.Specialites", "FaculteId", "dbo.Facultes");
            DropForeignKey("dbo.Specialites", "DepartementId", "dbo.Departements");
            DropForeignKey("dbo.Departements", "FaculteId", "dbo.Facultes");
            DropForeignKey("dbo.Courses", "CourseTypeId", "dbo.CourseTypes");
            DropForeignKey("dbo.Courses", "AnneeId", "dbo.Annees");
            DropForeignKey("dbo.ClassRooms", "FaculteId", "dbo.Facultes");
            DropForeignKey("dbo.ClassRooms", "ClassRoomTypeId", "dbo.ClassRoomTypes");
            DropIndex("dbo.Tcs", new[] { "GroupeId" });
            DropIndex("dbo.Tcs", new[] { "SectionId" });
            DropIndex("dbo.Tcs", new[] { "AnneeScolaireId" });
            DropIndex("dbo.Tcs", new[] { "CourseId" });
            DropIndex("dbo.Tcs", new[] { "TeacherId" });
            DropIndex("dbo.Seances", new[] { "TeacherId" });
            DropIndex("dbo.Sections", new[] { "AnneeScolaireId" });
            DropIndex("dbo.Sections", new[] { "SpecialiteId" });
            DropIndex("dbo.Sections", new[] { "AnneeId" });
            DropIndex("dbo.Groupes", new[] { "SectionId" });
            DropIndex("dbo.Departements", new[] { "FaculteId" });
            DropIndex("dbo.Specialites", new[] { "NiveauId" });
            DropIndex("dbo.Specialites", new[] { "FaculteId" });
            DropIndex("dbo.Specialites", new[] { "DepartementId" });
            DropIndex("dbo.Specialites", new[] { "FilliereId" });
            DropIndex("dbo.Courses", new[] { "AnneeId" });
            DropIndex("dbo.Courses", new[] { "SpecialiteId" });
            DropIndex("dbo.Courses", new[] { "CourseTypeId" });
            DropIndex("dbo.ClassRooms", new[] { "ClassRoomTypeId" });
            DropIndex("dbo.ClassRooms", new[] { "FaculteId" });
            DropTable("dbo.Tcs");
            DropTable("dbo.Teachers");
            DropTable("dbo.Seances");
            DropTable("dbo.Sections");
            DropTable("dbo.Groupes");
            DropTable("dbo.Niveaux");
            DropTable("dbo.Fillieres");
            DropTable("dbo.Departements");
            DropTable("dbo.Specialites");
            DropTable("dbo.CourseTypes");
            DropTable("dbo.Courses");
            DropTable("dbo.Facultes");
            DropTable("dbo.ClassRoomTypes");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.AnneeScolaires");
            DropTable("dbo.Annees");
        }
    }
}
