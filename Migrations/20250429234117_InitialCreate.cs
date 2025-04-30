using System;
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
                name: "Comptes",
                columns: table => new
                {
                    id_compte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MotDePasse = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    date_inscription = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptes", x => x.id_compte);
                });

            migrationBuilder.CreateTable(
                name: "Marque",
                columns: table => new
                {
                    id_marque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marque", x => x.id_marque);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    id_commande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date_commande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_reception = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_commande = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    adresse_livraison = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    id_compte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.id_commande);
                    table.ForeignKey(
                        name: "FK_Commandes_Comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "Comptes",
                        principalColumn: "id_compte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modele",
                columns: table => new
                {
                    id_modele = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_modele = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    nbr_neuf = table.Column<int>(type: "int", nullable: false),
                    nbr_occasion = table.Column<int>(type: "int", nullable: false),
                    prix_neuf = table.Column<decimal>(type: "decimal(18,2)", precision: 10, scale: 2, nullable: false),
                    prix_occasion = table.Column<decimal>(type: "decimal(18,2)", precision: 10, scale: 2, nullable: false),
                    Tag = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    id_marque = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modele", x => x.id_modele);
                    table.ForeignKey(
                        name: "FK_Modele_Marque_id_marque",
                        column: x => x.id_marque,
                        principalTable: "Marque",
                        principalColumn: "id_marque",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListeDeSouhaits",
                columns: table => new
                {
                    id_liste = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_modele = table.Column<int>(type: "int", nullable: false),
                    id_compte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeDeSouhaits", x => x.id_liste);
                    table.ForeignKey(
                        name: "FK_ListeDeSouhaits_Comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "Comptes",
                        principalColumn: "id_compte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListeDeSouhaits_Modele_id_modele",
                        column: x => x.id_modele,
                        principalTable: "Modele",
                        principalColumn: "id_modele",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    id_produit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Etat = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    id_commande = table.Column<int>(type: "int", nullable: true),
                    id_modele = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.id_produit);
                    table.ForeignKey(
                        name: "FK_Produits_Commandes_id_commande",
                        column: x => x.id_commande,
                        principalTable: "Commandes",
                        principalColumn: "id_commande",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produits_Modele_id_modele",
                        column: x => x.id_modele,
                        principalTable: "Modele",
                        principalColumn: "id_modele",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_date_commande",
                table: "Commandes",
                column: "date_commande");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_id_compte",
                table: "Commandes",
                column: "id_compte");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_status_commande",
                table: "Commandes",
                column: "status_commande");

            migrationBuilder.CreateIndex(
                name: "IX_Comptes_Email",
                table: "Comptes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListeDeSouhaits_id_compte",
                table: "ListeDeSouhaits",
                column: "id_compte");

            migrationBuilder.CreateIndex(
                name: "IX_ListeDeSouhaits_id_modele",
                table: "ListeDeSouhaits",
                column: "id_modele");

            migrationBuilder.CreateIndex(
                name: "IX_Marque_Nom",
                table: "Marque",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modele_id_marque",
                table: "Modele",
                column: "id_marque");

            migrationBuilder.CreateIndex(
                name: "IX_Modele_nom_modele",
                table: "Modele",
                column: "nom_modele");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_Etat",
                table: "Produits",
                column: "Etat");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_id_commande",
                table: "Produits",
                column: "id_commande");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_id_modele",
                table: "Produits",
                column: "id_modele");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListeDeSouhaits");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Modele");

            migrationBuilder.DropTable(
                name: "Comptes");

            migrationBuilder.DropTable(
                name: "Marque");
        }
    }
}
