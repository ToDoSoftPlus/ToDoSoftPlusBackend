using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTableMyDayListAddIsMyDayInItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyDayListEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsMayDay",
                table: "ToDoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMayDay",
                table: "ToDoItems");

            migrationBuilder.CreateTable(
                name: "MyDayListEntity",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ToDoItemId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDayListEntity", x => new { x.UserId, x.ToDoItemId, x.Date });
                    table.ForeignKey(
                        name: "FK_MyDayListEntity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyDayListEntity_ToDoItems_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyDayListEntity_ToDoItemId",
                table: "MyDayListEntity",
                column: "ToDoItemId");
        }
    }
}
