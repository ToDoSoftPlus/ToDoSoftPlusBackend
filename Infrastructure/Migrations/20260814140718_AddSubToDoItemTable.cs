using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubToDoItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoItems_ParentToDoItemId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_ParentToDoItemId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "ParentToDoItemId",
                table: "ToDoItems");

            migrationBuilder.CreateTable(
                name: "ToDoSubItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ToDoItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoSubItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoSubItems_ToDoItems_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoSubItems_ToDoItemId",
                table: "ToDoSubItems",
                column: "ToDoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoSubItems");

            migrationBuilder.AddColumn<int>(
                name: "ParentToDoItemId",
                table: "ToDoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_ParentToDoItemId",
                table: "ToDoItems",
                column: "ParentToDoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoItems_ParentToDoItemId",
                table: "ToDoItems",
                column: "ParentToDoItemId",
                principalTable: "ToDoItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
