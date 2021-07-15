namespace BigSchool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "UserName", c => c.String());
            AddColumn("dbo.Courses", "isShowGoing", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "isShowFollow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "isShowFollow");
            DropColumn("dbo.Courses", "isShowGoing");
            DropColumn("dbo.Courses", "UserName");
        }
    }
}
