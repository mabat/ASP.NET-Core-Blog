using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBlog.Data.Migrations
{
    public partial class PostUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_CreatedById",
                table: "Post",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_CreatedById",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_CreatedById",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Post");
        }
    }
}
