using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateLibrary.DAL.Data.Migrations.Library
{
    public partial class AddedLibraryCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LibraryCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookLibraryCustomer",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLibraryCustomer", x => new { x.BooksId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_BookLibraryCustomer_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLibraryCustomer_LibraryCustomers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "LibraryCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLibraryCustomer_CustomersId",
                table: "BookLibraryCustomer",
                column: "CustomersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLibraryCustomer");

            migrationBuilder.DropTable(
                name: "LibraryCustomers");
        }
    }
}
