using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MamisSolidarias.WebAPI.Donors.Migrations
{
    /// <inheritdoc />
    public partial class AddedDniAndMPFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Donors",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "Donors",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MercadoPagoEmail",
                table: "Donors",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donors_Dni",
                table: "Donors",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donors_MercadoPagoEmail",
                table: "Donors",
                column: "MercadoPagoEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Donors_Dni",
                table: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_Donors_MercadoPagoEmail",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "Dni",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "MercadoPagoEmail",
                table: "Donors");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Donors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
