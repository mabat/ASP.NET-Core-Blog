using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyBlog.Data.Migrations
{
    public partial class Gallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Post",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<byte[]>(nullable: true),
                    ContentType = table.Column<string>(maxLength: 100, nullable: true),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    GalleryId = table.Column<int>(nullable: true),
                    PostID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_File_Post_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Post",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_GalleryId",
                table: "File",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_File_PostID",
                table: "File",
                column: "PostID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Post");
        }
    }
}
