namespace Planing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pl31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "Nom", c => c.String(maxLength: 4000));
            AddColumn("dbo.Teachers", "Prenom", c => c.String(maxLength: 4000));
            DropColumn("dbo.Teachers", "NomComplet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "NomComplet", c => c.String(maxLength: 4000));
            DropColumn("dbo.Teachers", "Prenom");
            DropColumn("dbo.Teachers", "Nom");
        }
    }
}
