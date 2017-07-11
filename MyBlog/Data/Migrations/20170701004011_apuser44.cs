using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyBlog.Data.Migrations
{
    public partial class apuser44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Post",
                newName: "AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Post",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId1",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    AuthorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.AuthorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorId1",
                table: "Post",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Author_AuthorId1",
                table: "Post",
                column: "AuthorId1",
                principalTable: "Author",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Author_AuthorId1",
                table: "Post");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropIndex(
                name: "IX_Post_AuthorId1",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Post",
                newName: "ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Post",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
