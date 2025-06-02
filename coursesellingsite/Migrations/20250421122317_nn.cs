using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coursesellingsite.Migrations
{
    /// <inheritdoc />
    public partial class nn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContent_AddCoursePreviewDetails_PreviewDetailId",
                table: "CourseContent");

            migrationBuilder.DropIndex(
                name: "IX_CourseContent_PreviewDetailId",
                table: "CourseContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CourseContent_PreviewDetailId",
                table: "CourseContent",
                column: "PreviewDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContent_AddCoursePreviewDetails_PreviewDetailId",
                table: "CourseContent",
                column: "PreviewDetailId",
                principalTable: "AddCoursePreviewDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
