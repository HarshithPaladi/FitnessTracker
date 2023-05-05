using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    public partial class WorkoutVitals2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vitals_AspNetUsers_FitnessUserId",
                table: "Vitals");

            migrationBuilder.DropIndex(
                name: "IX_Vitals_FitnessUserId",
                table: "Vitals");

            migrationBuilder.AlterColumn<string>(
                name: "FitnessUserId",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FitnessUserId",
                table: "Vitals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_FitnessUserId",
                table: "Vitals",
                column: "FitnessUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vitals_AspNetUsers_FitnessUserId",
                table: "Vitals",
                column: "FitnessUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
