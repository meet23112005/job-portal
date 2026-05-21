using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Portal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeCanAlsoDeleteJobIfSomeOneSave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_Jobs_JobId",
                table: "SavedJobs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
