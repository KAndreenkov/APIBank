using Microsoft.EntityFrameworkCore.Migrations;

namespace APIBank.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyItems",
                columns: table => new
                {
                    CurrencyItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyShort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyLong = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyItems", x => x.CurrencyItemId);
                });

            migrationBuilder.CreateTable(
                name: "PersonNames",
                columns: table => new
                {
                    PersonNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonFam = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonNames", x => x.PersonNameId);
                });

            migrationBuilder.CreateTable(
                name: "BankRecords",
                columns: table => new
                {
                    BankRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonIdPersonNameId = table.Column<int>(type: "int", nullable: true),
                    CurrencyIdCurrencyItemId = table.Column<int>(type: "int", nullable: true),
                    Cash = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankRecords", x => x.BankRecordId);
                    table.ForeignKey(
                        name: "FK_BankRecords_CurrencyItems_CurrencyIdCurrencyItemId",
                        column: x => x.CurrencyIdCurrencyItemId,
                        principalTable: "CurrencyItems",
                        principalColumn: "CurrencyItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankRecords_PersonNames_PersonIdPersonNameId",
                        column: x => x.PersonIdPersonNameId,
                        principalTable: "PersonNames",
                        principalColumn: "PersonNameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankRecords_CurrencyIdCurrencyItemId",
                table: "BankRecords",
                column: "CurrencyIdCurrencyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BankRecords_PersonIdPersonNameId",
                table: "BankRecords",
                column: "PersonIdPersonNameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankRecords");

            migrationBuilder.DropTable(
                name: "CurrencyItems");

            migrationBuilder.DropTable(
                name: "PersonNames");
        }
    }
}
