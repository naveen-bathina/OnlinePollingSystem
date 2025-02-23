using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ops.Api.Migrations
{
    /// <inheritdoc />
    public partial class IncludedHasVoted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "Polls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VotedOptionId",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "VotedOptionId",
                table: "Polls");
        }
    }
}
