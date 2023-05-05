using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    public partial class WorkoutVitals1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_AspNetUsers_FitnessUserId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_FitnessUserId",
                table: "Workouts");

            migrationBuilder.AlterColumn<string>(
                name: "FitnessUserId",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FitnessUserId",
                table: "Workouts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_FitnessUserId",
                table: "Workouts",
                column: "FitnessUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_AspNetUsers_FitnessUserId",
                table: "Workouts",
                column: "FitnessUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
