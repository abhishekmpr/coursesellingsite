using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coursesellingsite.Migrations
{
    /// <inheritdoc />
    public partial class D : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddCoursePreviewDetails_CourseTitles_CourseTitleId",
                table: "AddCoursePreviewDetails");

            migrationBuilder.DropTable(
                name: "CourseCategories");

            migrationBuilder.DropTable(
                name: "CourseTitles");

            migrationBuilder.DropIndex(
                name: "IX_AddCoursePreviewDetails_CourseTitleId",
                table: "AddCoursePreviewDetails");

            migrationBuilder.DropColumn(
                name: "CourseTitleId",
                table: "AddCoursePreviewDetails");

            migrationBuilder.AddColumn<string>(
                name: "Pic",
                table: "CourseContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreviewDetailId",
                table: "CourseContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "CourseContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseTitle",
                table: "AddCoursePreviewDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pic",
                table: "AddCoursePreviewDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_PreviewDetailId",
                table: "CourseContents",
                column: "PreviewDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContents_AddCoursePreviewDetails_PreviewDetailId",
                table: "CourseContents",
                column: "PreviewDetailId",
                principalTable: "AddCoursePreviewDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContents_AddCoursePreviewDetails_PreviewDetailId",
                table: "CourseContents");

            migrationBuilder.DropIndex(
                name: "IX_CourseContents_PreviewDetailId",
                table: "CourseContents");

            migrationBuilder.DropColumn(
                name: "Pic",
                table: "CourseContents");

            migrationBuilder.DropColumn(
                name: "PreviewDetailId",
                table: "CourseContents");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "CourseContents");

            migrationBuilder.DropColumn(
                name: "CourseTitle",
                table: "AddCoursePreviewDetails");

            migrationBuilder.DropColumn(
                name: "Pic",
                table: "AddCoursePreviewDetails");

            migrationBuilder.AddColumn<int>(
                name: "CourseTitleId",
                table: "AddCoursePreviewDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTitles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddCoursePreviewDetails_CourseTitleId",
                table: "AddCoursePreviewDetails",
                column: "CourseTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddCoursePreviewDetails_CourseTitles_CourseTitleId",
                table: "AddCoursePreviewDetails",
                column: "CourseTitleId",
                principalTable: "CourseTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
