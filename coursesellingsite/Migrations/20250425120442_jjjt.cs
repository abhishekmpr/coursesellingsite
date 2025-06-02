using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coursesellingsite.Migrations
{
    /// <inheritdoc />
    public partial class jjjt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId",
                table: "UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_UserId",
                table: "UserCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_AddCoursePreviewDetails_CourseId",
                table: "UserCourses",
                column: "CourseId",
                principalTable: "AddCoursePreviewDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Registers_UserId",
                table: "UserCourses",
                column: "UserId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_AddCoursePreviewDetails_CourseId",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Registers_UserId",
                table: "UserCourses");

            migrationBuilder.DropIndex(
                name: "IX_UserCourses_CourseId",
                table: "UserCourses");

            migrationBuilder.DropIndex(
                name: "IX_UserCourses_UserId",
                table: "UserCourses");
        }
    }
}
