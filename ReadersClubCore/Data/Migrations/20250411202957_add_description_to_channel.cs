using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadersClubCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_description_to_channel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Channels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Channels");
        }
    }
}
