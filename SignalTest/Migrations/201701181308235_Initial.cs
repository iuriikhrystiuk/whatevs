namespace SignalTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DummyInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Information = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DummyInformations");
        }
    }
}
