using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class cloudDbMigv21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderTable_FolderTable_ParentFolderId",
                table: "FolderTable");

            migrationBuilder.AddForeignKey(
                name: "FK_FolderTable_FolderTable_ParentFolderId",
                table: "FolderTable",
                column: "ParentFolderId",
                principalTable: "FolderTable",
                principalColumn: "FolderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderTable_FolderTable_ParentFolderId",
                table: "FolderTable");

            migrationBuilder.AddForeignKey(
                name: "FK_FolderTable_FolderTable_ParentFolderId",
                table: "FolderTable",
                column: "ParentFolderId",
                principalTable: "FolderTable",
                principalColumn: "FolderId");
        }
    }
}
