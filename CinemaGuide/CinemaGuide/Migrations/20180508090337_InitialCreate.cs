using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaGuide.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Profiles",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.SerialColumn),
                    Age = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Role = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Profiles", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HashedPassword = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    ProfileForeignKey = table.Column<int>(nullable: false),
                    Salt = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        "FK_Users_Profiles_ProfileForeignKey",
                        x => x.ProfileForeignKey,
                        "Profiles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Users_ProfileForeignKey",
                "Users",
                "ProfileForeignKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Users");
            migrationBuilder.DropTable("Profiles");
        }
    }
}
