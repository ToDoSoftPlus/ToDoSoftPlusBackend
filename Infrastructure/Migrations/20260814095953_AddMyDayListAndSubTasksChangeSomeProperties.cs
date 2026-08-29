using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMyDayListAndSubTasksChangeSomeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId",
                table: "ToDoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_Categories_ToDoCategoryEntityId",
                table: "ToDoItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_Id",
                table: "ToDoItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ToDoItems",
                newName: "ToDoListId");

            migrationBuilder.RenameColumn(
                name: "ToDoCategoryEntityId",
                table: "ToDoItems",
                newName: "ParentToDoItemId");

            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "ToDoItems",
                newName: "IsCompleted");

            migrationBuilder.RenameColumn(
                name: "DeterminateDate",
                table: "ToDoItems",
                newName: "CompletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_UserId",
                table: "ToDoItems",
                newName: "IX_ToDoItems_ToDoListId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_ToDoCategoryEntityId",
                table: "ToDoItems",
                newName: "IX_ToDoItems_ParentToDoItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ToDoItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ToDoItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.CreateTable(
                name: "ToDoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_IsCompleted",
                table: "ToDoItems",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_MyDayListEntity_ToDoItemId",
                table: "MyDayListEntity",
                column: "ToDoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoLists_UserId",
                table: "ToDoLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoItems_ParentToDoItemId",
                table: "ToDoItems",
                column: "ParentToDoItemId",
                principalTable: "ToDoItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoLists_ToDoListId",
                table: "ToDoItems",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoItems_ParentToDoItemId",
                table: "ToDoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoLists_ToDoListId",
                table: "ToDoItems");

            migrationBuilder.DropTable(
                name: "MyDayListEntity");

            migrationBuilder.DropTable(
                name: "ToDoLists");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_IsCompleted",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ToDoListId",
                table: "ToDoItems",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ParentToDoItemId",
                table: "ToDoItems",
                newName: "ToDoCategoryEntityId");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "ToDoItems",
                newName: "IsComplete");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "ToDoItems",
                newName: "DeterminateDate");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_ToDoListId",
                table: "ToDoItems",
                newName: "IX_ToDoItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDoItems_ParentToDoItemId",
                table: "ToDoItems",
                newName: "IX_ToDoItems_ToDoCategoryEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ToDoItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_Id",
                table: "ToDoItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Id",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_AspNetUsers_UserId",
                table: "ToDoItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_Categories_ToDoCategoryEntityId",
                table: "ToDoItems",
                column: "ToDoCategoryEntityId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
