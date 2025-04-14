#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.DbMigrations;

/// <inheritdoc />
public partial class added_refresh_token : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Roles",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>("text", nullable: true)
            },
            constraints: table => { table.PrimaryKey("PK_Roles", x => x.Id); });

        migrationBuilder.CreateTable(
            "Users",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                RefreshToken = table.Column<string>("text", nullable: true),
                RefreshTokenExpiryTime = table.Column<DateTime>("timestamp with time zone", nullable: false),
                AboutMe_FirstName = table.Column<string>("character varying(50)", maxLength: 50, nullable: true),
                AboutMe_LastName = table.Column<string>("character varying(50)", maxLength: 50, nullable: true),
                AboutMe_Headline = table.Column<string>("character varying(250)", maxLength: 250, nullable: true),
                AboutMe_ProfilePicture =
                    table.Column<string>("character varying(2000)", maxLength: 2000, nullable: true),
                AboutMe_Bio = table.Column<string>("character varying(2500)", maxLength: 2500, nullable: true),
                AboutMe_Country = table.Column<string>("character varying(50)", maxLength: 50, nullable: true),
                AboutMe_City = table.Column<string>("character varying(50)", maxLength: 50, nullable: true),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("boolean", nullable: false),
                PasswordHash = table.Column<string>("text", nullable: true),
                SecurityStamp = table.Column<string>("text", nullable: true),
                ConcurrencyStamp = table.Column<string>("text", nullable: true),
                PhoneNumber = table.Column<string>("text", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("boolean", nullable: false),
                TwoFactorEnabled = table.Column<bool>("boolean", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("timestamp with time zone", nullable: true),
                LockoutEnabled = table.Column<bool>("boolean", nullable: false),
                AccessFailedCount = table.Column<int>("integer", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

        migrationBuilder.CreateTable(
            "RoleClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RoleId = table.Column<Guid>("uuid", nullable: false),
                ClaimType = table.Column<string>("text", nullable: true),
                ClaimValue = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
                table.ForeignKey(
                    "FK_RoleClaims_Roles_RoleId",
                    x => x.RoleId,
                    "Roles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Educations",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                School = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                Degree = table.Column<string>("character varying(50)", maxLength: 50, nullable: false),
                Location = table.Column<string>("character varying(250)", maxLength: 250, nullable: false),
                FieldOfStudy = table.Column<string>("character varying(250)", maxLength: 250, nullable: true),
                StartDate = table.Column<DateOnly>("date", maxLength: 10, nullable: false),
                EndDate = table.Column<DateOnly>("date", maxLength: 10, nullable: true),
                Description = table.Column<string>("character varying(2500)", maxLength: 2500, nullable: true),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Educations", x => x.Id);
                table.ForeignKey(
                    "FK_Educations_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Experiences",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Title = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                CompanyName = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                Location = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                StartDate = table.Column<DateOnly>("date", maxLength: 10, nullable: false),
                EndDate = table.Column<DateOnly>("date", maxLength: 10, nullable: true),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: true),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Experiences", x => x.Id);
                table.ForeignKey(
                    "FK_Experiences_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Messages",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Name = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>("character varying(320)", maxLength: 320, nullable: false),
                Subject = table.Column<string>("character varying(250)", maxLength: 250, nullable: true),
                Content = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: false),
                SentAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                IsRead = table.Column<bool>("boolean", nullable: false),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    "FK_Messages_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Projects",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Name = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>("character varying(1000)", maxLength: 1000, nullable: false),
                Url = table.Column<string>("character varying(50)", maxLength: 50, nullable: true),
                GithubUrl = table.Column<string>("character varying(50)", maxLength: 50, nullable: false),
                Skills = table.Column<List<string>>("text[]", maxLength: 2500, nullable: false),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
                table.ForeignKey(
                    "FK_Projects_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Skills",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                SkillCategory = table.Column<string>("character varying(100)", maxLength: 100, nullable: false),
                SkillsTypes = table.Column<List<string>>("text[]", maxLength: 2500, nullable: false),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Skills", x => x.Id);
                table.ForeignKey(
                    "FK_Skills_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "SocialLinks",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Platform = table.Column<string>("character varying(50)", maxLength: 50, nullable: false),
                Url = table.Column<string>("character varying(2000)", maxLength: 2000, nullable: false),
                Icon = table.Column<string>("character varying(100)", maxLength: 100, nullable: true),
                UserId = table.Column<Guid>("uuid", nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialLinks", x => x.Id);
                table.ForeignKey(
                    "FK_SocialLinks_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserClaims",
            table => new
            {
                Id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<Guid>("uuid", nullable: false),
                ClaimType = table.Column<string>("text", nullable: true),
                ClaimValue = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
                table.ForeignKey(
                    "FK_UserClaims_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("text", nullable: false),
                ProviderKey = table.Column<string>("text", nullable: false),
                ProviderDisplayName = table.Column<string>("text", nullable: true),
                UserId = table.Column<Guid>("uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    "FK_UserLogins_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserRoles",
            table => new
            {
                UserId = table.Column<Guid>("uuid", nullable: false),
                RoleId = table.Column<Guid>("uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    "FK_UserRoles_Roles_RoleId",
                    x => x.RoleId,
                    "Roles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_UserRoles_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserTokens",
            table => new
            {
                UserId = table.Column<Guid>("uuid", nullable: false),
                LoginProvider = table.Column<string>("text", nullable: false),
                Name = table.Column<string>("text", nullable: false),
                Value = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    "FK_UserTokens_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Educations_UserId",
            "Educations",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_Experiences_UserId",
            "Experiences",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_Messages_UserId",
            "Messages",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_Projects_UserId",
            "Projects",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_RoleClaims_RoleId",
            "RoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "Roles",
            "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Skills_UserId",
            "Skills",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_SocialLinks_UserId",
            "SocialLinks",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserClaims_UserId",
            "UserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserLogins_UserId",
            "UserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserRoles_RoleId",
            "UserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "Users",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "Users",
            "NormalizedUserName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Educations");

        migrationBuilder.DropTable(
            "Experiences");

        migrationBuilder.DropTable(
            "Messages");

        migrationBuilder.DropTable(
            "Projects");

        migrationBuilder.DropTable(
            "RoleClaims");

        migrationBuilder.DropTable(
            "Skills");

        migrationBuilder.DropTable(
            "SocialLinks");

        migrationBuilder.DropTable(
            "UserClaims");

        migrationBuilder.DropTable(
            "UserLogins");

        migrationBuilder.DropTable(
            "UserRoles");

        migrationBuilder.DropTable(
            "UserTokens");

        migrationBuilder.DropTable(
            "Roles");

        migrationBuilder.DropTable(
            "Users");
    }
}