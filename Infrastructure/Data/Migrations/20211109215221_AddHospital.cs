using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddHospital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_DoctorHospitals_DoctorHospitalId",
                table: "Offices");

            migrationBuilder.RenameColumn(
                name: "DoctorHospitalId",
                table: "Offices",
                newName: "HospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_DoctorHospitalId",
                table: "Offices",
                newName: "IX_Offices_HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Hospitals_HospitalId",
                table: "Offices",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Hospitals_HospitalId",
                table: "Offices");

            migrationBuilder.RenameColumn(
                name: "HospitalId",
                table: "Offices",
                newName: "DoctorHospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_HospitalId",
                table: "Offices",
                newName: "IX_Offices_DoctorHospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_DoctorHospitals_DoctorHospitalId",
                table: "Offices",
                column: "DoctorHospitalId",
                principalTable: "DoctorHospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
