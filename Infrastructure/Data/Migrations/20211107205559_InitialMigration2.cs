using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_HospitalCorrelations_HospitalAffiliationId",
                table: "Offices");

            migrationBuilder.DropTable(
                name: "HospitalCorrelations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DoctorSpecialties");

            migrationBuilder.RenameColumn(
                name: "HospitalAffiliationId",
                table: "Offices",
                newName: "DoctorHospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_HospitalAffiliationId",
                table: "Offices",
                newName: "IX_Offices_DoctorHospitalId");

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorHospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorHospitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorHospitals_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorHospitals_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHospitals_DoctorId",
                table: "DoctorHospitals",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHospitals_HospitalId",
                table: "DoctorHospitals",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_DoctorHospitals_DoctorHospitalId",
                table: "Offices",
                column: "DoctorHospitalId",
                principalTable: "DoctorHospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_DoctorHospitals_DoctorHospitalId",
                table: "Offices");

            migrationBuilder.DropTable(
                name: "DoctorHospitals");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.RenameColumn(
                name: "DoctorHospitalId",
                table: "Offices",
                newName: "HospitalAffiliationId");

            migrationBuilder.RenameIndex(
                name: "IX_Offices_DoctorHospitalId",
                table: "Offices",
                newName: "IX_Offices_HospitalAffiliationId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DoctorSpecialties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HospitalCorrelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalCorrelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalCorrelations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HospitalCorrelations_DoctorId",
                table: "HospitalCorrelations",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_HospitalCorrelations_HospitalAffiliationId",
                table: "Offices",
                column: "HospitalAffiliationId",
                principalTable: "HospitalCorrelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
