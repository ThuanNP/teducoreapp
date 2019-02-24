using Microsoft.EntityFrameworkCore.Migrations;

namespace TeduCoreApp.Data.EF.Migrations
{
    public partial class updateBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HomeFlag",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HotFlag",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Blogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeFlag",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "HotFlag",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Blogs");
        }
    }
}
