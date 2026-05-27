using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerCleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskManagerCleanArchitectureAddingJsonSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalData",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalData",
                table: "Tasks");
        }
    }
}
