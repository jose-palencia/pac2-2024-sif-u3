using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IShop.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sec_roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sec_users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PasswordLoss = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordLossDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    gender = table.Column<string>(type: "TEXT", maxLength: 1, nullable: true),
                    birth_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    job_title = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    identity = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    refresh_token = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    refresh_token_expiry = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Version = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sec_roles_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_roles_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sec_roles_claims_sec_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sec_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sec_users_claims_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_sec_users_logins_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_roles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    GrantedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GrantedById = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_sec_users_roles_sec_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sec_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sec_users_roles_sec_users_GrantedById",
                        column: x => x.GrantedById,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sec_users_roles_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sec_users_tokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sec_users_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_sec_users_tokens_sec_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sec_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "sec_roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sec_roles_claims_RoleId",
                table: "sec_roles_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "sec_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "sec_users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_claims_UserId",
                table: "sec_users_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_logins_UserId",
                table: "sec_users_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_roles_GrantedById",
                table: "sec_users_roles",
                column: "GrantedById");

            migrationBuilder.CreateIndex(
                name: "IX_sec_users_roles_RoleId",
                table: "sec_users_roles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sec_roles_claims");

            migrationBuilder.DropTable(
                name: "sec_users_claims");

            migrationBuilder.DropTable(
                name: "sec_users_logins");

            migrationBuilder.DropTable(
                name: "sec_users_roles");

            migrationBuilder.DropTable(
                name: "sec_users_tokens");

            migrationBuilder.DropTable(
                name: "sec_roles");

            migrationBuilder.DropTable(
                name: "sec_users");
        }
    }
}
