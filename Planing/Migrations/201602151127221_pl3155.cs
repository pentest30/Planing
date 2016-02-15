namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl3155 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CourseTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "CourseTypeId");
            AddForeignKey("dbo.Courses", "CourseTypeId", "dbo.CourseTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "CourseTypeId", "dbo.CourseTypes");
            DropIndex("dbo.Courses", new[] { "CourseTypeId" });
            DropColumn("dbo.Courses", "CourseTypeId");
        }
    }
}
