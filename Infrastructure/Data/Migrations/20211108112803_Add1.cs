using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Add1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtiesId",
                table: "DoctorSpecialties");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSpecialties_SpecialtiesId",
                table: "DoctorSpecialties");

            migrationBuilder.DropColumn(
                name: "SpecialtiesId",
                table: "DoctorSpecialties");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                table: "DoctorSpecialties",
                newName: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                table: "DoctorSpecialties",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                table: "DoctorSpecialties",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                table: "DoctorSpecialties");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                table: "DoctorSpecialties");

            migrationBuilder.RenameColumn(
                name: "SpecialtyId",
                table: "DoctorSpecialties",
                newName: "SpecializationId");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtiesId",
                table: "DoctorSpecialties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtiesId",
                table: "DoctorSpecialties",
                column: "SpecialtiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtiesId",
                table: "DoctorSpecialties",
                column: "SpecialtiesId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
