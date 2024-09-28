using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD1_code.Migrations
{
    /// <inheritdoc />
    public partial class CrationDBContexte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marque",
                columns: table => new
                {
                    idMarque = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomMarque = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produit2", x => x.idMarque);
                });

            migrationBuilder.CreateTable(
                name: "TypeProduit",
                columns: table => new
                {
                    idTypeProduit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomTypeProduit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_typeProduit", x => x.idTypeProduit);
                });

            migrationBuilder.CreateTable(
                name: "Produit",
                columns: table => new
                {
                    idTypeProduit = table.Column<int>(type: "integer", nullable: false),
                    idMarque = table.Column<int>(type: "integer", nullable: false),
                    IdProduit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomProduit = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    nomPhoto = table.Column<string>(type: "text", nullable: false),
                    uriPhoto = table.Column<string>(type: "text", nullable: false),
                    stockReel = table.Column<int>(type: "integer", nullable: false),
                    stockMin = table.Column<int>(type: "integer", nullable: false),
                    stockMax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produit", x => new { x.idMarque, x.idTypeProduit });
                    table.ForeignKey(
                        name: "fk_produit_marque",
                        column: x => x.idMarque,
                        principalTable: "Marque",
                        principalColumn: "idMarque");
                    table.ForeignKey(
                        name: "fk_produit_typeProduit",
                        column: x => x.idTypeProduit,
                        principalTable: "TypeProduit",
                        principalColumn: "idTypeProduit");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produit_idTypeProduit",
                table: "Produit",
                column: "idTypeProduit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produit");

            migrationBuilder.DropTable(
                name: "Marque");

            migrationBuilder.DropTable(
                name: "TypeProduit");
        }
    }
}
