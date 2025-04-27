using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendGrenishop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entreprises",
                columns: table => new
                {
                    EntrepriseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Entreprise = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entreprises", x => x.EntrepriseID);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    ProduitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntrepriseID = table.Column<int>(type: "int", nullable: false),
                    Nombre_Neuf = table.Column<int>(type: "int", nullable: false),
                    Nombre_Occasion = table.Column<int>(type: "int", nullable: false),
                    Prix_Neuf = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Prix_Occasion = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.ProduitID);
                    table.ForeignKey(
                        name: "FK_Produits_Entreprises_EntrepriseID",
                        column: x => x.EntrepriseID,
                        principalTable: "Entreprises",
                        principalColumn: "EntrepriseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produits_EntrepriseID",
                table: "Produits",
                column: "EntrepriseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Entreprises");
        }
    }
}
