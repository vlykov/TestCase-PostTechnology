using Microsoft.EntityFrameworkCore.Migrations;

namespace PostTechnology.DataAccess.EntityFramework.Migrations
{
    public partial class SetHashColumnNotUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SentMessages_Hash",
                table: "SentMessages");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMessages_Hash",
                table: "ReceivedMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "SentMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "ReceivedMessages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "SentMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "ReceivedMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_SentMessages_Hash",
                table: "SentMessages",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMessages_Hash",
                table: "ReceivedMessages",
                column: "Hash",
                unique: true);
        }
    }
}
