using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Data.Migrations
{
    public partial class removegallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
