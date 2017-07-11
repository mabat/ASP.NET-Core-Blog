using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Data.Migrations
{
    public partial class apuser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Post",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CreatedById",
                table: "Post",
                newName: "IX_Post_ApplicationUserId");

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

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Post",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post",
                newName: "IX_Post_CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
