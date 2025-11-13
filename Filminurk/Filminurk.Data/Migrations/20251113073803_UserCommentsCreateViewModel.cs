using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filminurk.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserCommentsCreateViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommetredScore",
                table: "UserComments",
                newName: "CommentedScore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentedScore",
                table: "UserComments",
                newName: "CommetredScore");
        }
    }
}
