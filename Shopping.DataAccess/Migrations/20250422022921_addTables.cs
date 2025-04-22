using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingMVC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Description", "ExpiryDate", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, "Now’s the perfect time to grow your bookshelf! Enjoy 20% off all fiction titles—whether you're chasing epic adventures or cozy reads. Your next favorite book is just a page away.", new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Local), "", "🔥 20% Off All Fiction Books!" },
                    { 2, "Treat yourself to some new books! Buy any two, and we’ll throw in a third one for free. From exciting adventures to heartfelt stories, there’s something for every reader. Don’t miss out—stock up today!", new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Local), "", "📚 Buy 2 Get 1 Free!" },
                    { 3, "Enjoy the convenience of free shipping when you spend $50 or more! It’s the perfect time to shop your favorites and have them delivered straight to your door, on us. Don't wait—free shipping is just a few clicks away!", new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Local), "", "🎉 Free Shipping Over $50!" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
