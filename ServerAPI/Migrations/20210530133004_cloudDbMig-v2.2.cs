using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerAPI.Migrations
{
    public partial class cloudDbMigv22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "FileTable");

            migrationBuilder.RenameColumn(
                name: "TruePath",
                table: "FileTable",
                newName: "VirtualPath");

            migrationBuilder.CreateTable(
                name: "FilesContentTable",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilesDataId = table.Column<int>(type: "int", nullable: false),
                    FileBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesContentTable", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_FilesContentTable_FileTable_FilesDataId",
                        column: x => x.FilesDataId,
                        principalTable: "FileTable",
                        principalColumn: "FilesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilesContentTable_FilesDataId",
                table: "FilesContentTable",
                column: "FilesDataId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesContentTable");

            migrationBuilder.RenameColumn(
                name: "VirtualPath",
                table: "FileTable",
                newName: "TruePath");

            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "FileTable",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
