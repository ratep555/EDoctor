using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddHospital1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals");

            migrationBuilder.DropIndex(
                name: "IX_DoctorHospitals_DoctorId",
                table: "DoctorHospitals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DoctorHospitals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals",
                columns: new[] { "DoctorId", "HospitalId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DoctorHospitals",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorHospitals",
                table: "DoctorHospitals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHospitals_DoctorId",
                table: "DoctorHospitals",
                column: "DoctorId");
        }
    }
}
