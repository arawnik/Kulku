using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class AddNetworkEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CompanyNetworkProfiles",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                Stage = table.Column<string>(type: "text", nullable: false),
                Notes = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyNetworkProfiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_CompanyNetworkProfiles_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "NetworkCategories",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                ColorToken = table.Column<string>(
                    type: "character varying(50)",
                    maxLength: 50,
                    nullable: true
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NetworkCategories", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "NetworkContacts",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                PersonName = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Email = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Phone = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                LinkedInUrl = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Title = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                CreatedAt = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NetworkContacts", x => x.Id);
                table.ForeignKey(
                    name: "FK_NetworkContacts_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "CompanyNetworkProfileCategories",
            columns: table => new
            {
                CompanyNetworkProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                NetworkCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_CompanyNetworkProfileCategories",
                    x => new { x.CompanyNetworkProfileId, x.NetworkCategoryId }
                );
                table.ForeignKey(
                    name: "FK_CompanyNetworkProfileCategories_CompanyNetworkProfiles_Comp~",
                    column: x => x.CompanyNetworkProfileId,
                    principalTable: "CompanyNetworkProfiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_CompanyNetworkProfileCategories_NetworkCategories_NetworkCa~",
                    column: x => x.NetworkCategoryId,
                    principalTable: "NetworkCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "NetworkInteractions",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                ContactId = table.Column<Guid>(type: "uuid", nullable: true),
                Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Direction = table.Column<string>(type: "text", nullable: false),
                Channel = table.Column<string>(type: "text", nullable: false),
                IsWarmIntro = table.Column<bool>(type: "boolean", nullable: false),
                ReferredByName = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                ReferredByRelation = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Summary = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                NextAction = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                NextActionDue = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: true
                ),
                CreatedAt = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NetworkInteractions", x => x.Id);
                table.ForeignKey(
                    name: "FK_NetworkInteractions_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_NetworkInteractions_NetworkContacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "NetworkContacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyNetworkProfileCategories_NetworkCategoryId",
            table: "CompanyNetworkProfileCategories",
            column: "NetworkCategoryId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyNetworkProfiles_CompanyId",
            table: "CompanyNetworkProfiles",
            column: "CompanyId",
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_NetworkContacts_CompanyId",
            table: "NetworkContacts",
            column: "CompanyId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_NetworkInteractions_CompanyId",
            table: "NetworkInteractions",
            column: "CompanyId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_NetworkInteractions_ContactId",
            table: "NetworkInteractions",
            column: "ContactId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_NetworkInteractions_Date",
            table: "NetworkInteractions",
            column: "Date"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "CompanyNetworkProfileCategories");

        migrationBuilder.DropTable(name: "NetworkInteractions");

        migrationBuilder.DropTable(name: "CompanyNetworkProfiles");

        migrationBuilder.DropTable(name: "NetworkCategories");

        migrationBuilder.DropTable(name: "NetworkContacts");
    }
}
