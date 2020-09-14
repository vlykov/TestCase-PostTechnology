using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PostTechnology.DataAccess.EntityFramework.Migrations
{
    public partial class СompositionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.CreateTable(
                name: "ReceivedMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Sent = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Hash = table.Column<string>(nullable: false),
                    Received = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SentMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Sent = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Hash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMessages_Hash",
                table: "ReceivedMessages",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMessages_Number",
                table: "ReceivedMessages",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SentMessages_Hash",
                table: "SentMessages",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SentMessages_Number",
                table: "SentMessages",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivedMessages");

            migrationBuilder.DropTable(
                name: "SentMessages");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Sent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Hash",
                table: "Messages",
                column: "Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Number",
                table: "Messages",
                column: "Number",
                unique: true);
        }
    }
}
