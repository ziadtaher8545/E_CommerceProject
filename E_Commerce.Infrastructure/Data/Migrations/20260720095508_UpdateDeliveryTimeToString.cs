using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryTimeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryTime",
                table: "DeliveryMethods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeliveryTime",
                table: "DeliveryMethods",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
