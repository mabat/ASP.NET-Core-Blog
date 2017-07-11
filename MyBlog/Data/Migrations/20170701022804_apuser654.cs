using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyBlog.Data.Migrations
{
    public partial class apuser654 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "ApplicationUserID");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "Post",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_ApplicationUserID",
                table: "Post",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserID",
                table: "Post",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserID",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_ApplicationUserID",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
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
    }
}
