﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Penkoff.Storage.Migrations
{
    public partial class FixCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cards");
        }
    }
}
