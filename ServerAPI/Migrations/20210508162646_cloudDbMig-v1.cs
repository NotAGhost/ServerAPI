using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class cloudDbMigv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FolderTable",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentFolderId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TruePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderTable", x => x.FolderId);
                    table.ForeignKey(
                        name: "FK_FolderTable_FolderTable_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "FolderTable",
                        principalColumn: "FolderId");
                });

            migrationBuilder.CreateTable(
                name: "FileTable",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentFolderId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TruePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTable", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileTable_FolderTable_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "FolderTable",
                        principalColumn: "FolderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileTable_ParentFolderId",
                table: "FileTable",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderTable_ParentFolderId",
                table: "FolderTable",
                column: "ParentFolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileTable");

            migrationBuilder.DropTable(
                name: "FolderTable");
        }
    }
}
