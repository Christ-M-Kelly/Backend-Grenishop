using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendGrenishop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comptes",
                columns: table => new
                {
                    id_compte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasseHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_inscription = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptes", x => x.id_compte);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comptes");
        }
    }
}
