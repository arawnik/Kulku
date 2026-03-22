using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class AddIdeaBank : Migration
{
    private static readonly string[] _ideaDomainTranslationIndexColumns =
    [
        "IdeaDomainId",
        "Language",
    ];

    private static readonly string[] _ideaPriorityTranslationIndexColumns =
    [
        "IdeaPriorityId",
        "Language",
    ];

    private static readonly string[] _ideaStatusTranslationIndexColumns =
    [
        "IdeaStatusId",
        "Language",
    ];

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "IdeaDomains",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Icon = table.Column<string>(
                    type: "character varying(50)",
                    maxLength: 50,
                    nullable: false
                ),
                Order = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaDomains", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaPriorities",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Order = table.Column<int>(type: "integer", nullable: false),
                Style = table.Column<string>(
                    type: "character varying(50)",
                    maxLength: 50,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaPriorities", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaStatuses",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Order = table.Column<int>(type: "integer", nullable: false),
                Style = table.Column<string>(
                    type: "character varying(50)",
                    maxLength: 50,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaStatuses", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaTags",
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
                ColorHex = table.Column<string>(
                    type: "character varying(9)",
                    maxLength: 9,
                    nullable: true
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaTags", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaDomainTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                IdeaDomainId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Language = table.Column<string>(
                    type: "character varying(2)",
                    unicode: false,
                    maxLength: 2,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaDomainTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdeaDomainTranslations_IdeaDomains_IdeaDomainId",
                    column: x => x.IdeaDomainId,
                    principalTable: "IdeaDomains",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaPriorityTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                IdeaPriorityId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
                Language = table.Column<string>(
                    type: "character varying(2)",
                    unicode: false,
                    maxLength: 2,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaPriorityTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdeaPriorityTranslations_IdeaPriorities_IdeaPriorityId",
                    column: x => x.IdeaPriorityId,
                    principalTable: "IdeaPriorities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "Ideas",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Title = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Summary = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
                StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                PriorityId = table.Column<Guid>(type: "uuid", nullable: false),
                DomainId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAt = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
                UpdatedAt = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ideas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Ideas_IdeaDomains_DomainId",
                    column: x => x.DomainId,
                    principalTable: "IdeaDomains",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
                table.ForeignKey(
                    name: "FK_Ideas_IdeaPriorities_PriorityId",
                    column: x => x.PriorityId,
                    principalTable: "IdeaPriorities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
                table.ForeignKey(
                    name: "FK_Ideas_IdeaStatuses_StatusId",
                    column: x => x.StatusId,
                    principalTable: "IdeaStatuses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaStatusTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                IdeaStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
                Language = table.Column<string>(
                    type: "character varying(2)",
                    unicode: false,
                    maxLength: 2,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaStatusTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdeaStatusTranslations_IdeaStatuses_IdeaStatusId",
                    column: x => x.IdeaStatusId,
                    principalTable: "IdeaStatuses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaIdeaTag",
            columns: table => new
            {
                IdeaId = table.Column<Guid>(type: "uuid", nullable: false),
                IdeaTagId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaIdeaTag", x => new { x.IdeaId, x.IdeaTagId });
                table.ForeignKey(
                    name: "FK_IdeaIdeaTag_IdeaTags_IdeaTagId",
                    column: x => x.IdeaTagId,
                    principalTable: "IdeaTags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_IdeaIdeaTag_Ideas_IdeaId",
                    column: x => x.IdeaId,
                    principalTable: "Ideas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaKeyword",
            columns: table => new
            {
                IdeaId = table.Column<Guid>(type: "uuid", nullable: false),
                KeywordId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaKeyword", x => new { x.IdeaId, x.KeywordId });
                table.ForeignKey(
                    name: "FK_IdeaKeyword_Ideas_IdeaId",
                    column: x => x.IdeaId,
                    principalTable: "Ideas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_IdeaKeyword_Keywords_KeywordId",
                    column: x => x.KeywordId,
                    principalTable: "Keywords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IdeaNotes",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                IdeaId = table.Column<Guid>(type: "uuid", nullable: false),
                Content = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                CreatedAt = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IdeaNotes", x => x.Id);
                table.ForeignKey(
                    name: "FK_IdeaNotes_Ideas_IdeaId",
                    column: x => x.IdeaId,
                    principalTable: "Ideas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_IdeaDomainTranslations_IdeaDomainId_Language",
            table: "IdeaDomainTranslations",
            columns: _ideaDomainTranslationIndexColumns,
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_IdeaIdeaTag_IdeaTagId",
            table: "IdeaIdeaTag",
            column: "IdeaTagId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IdeaKeyword_KeywordId",
            table: "IdeaKeyword",
            column: "KeywordId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IdeaNotes_IdeaId",
            table: "IdeaNotes",
            column: "IdeaId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IdeaPriorityTranslations_IdeaPriorityId_Language",
            table: "IdeaPriorityTranslations",
            columns: _ideaPriorityTranslationIndexColumns,
            unique: true
        );

        migrationBuilder.CreateIndex(name: "IX_Ideas_DomainId", table: "Ideas", column: "DomainId");

        migrationBuilder.CreateIndex(
            name: "IX_Ideas_PriorityId",
            table: "Ideas",
            column: "PriorityId"
        );

        migrationBuilder.CreateIndex(name: "IX_Ideas_StatusId", table: "Ideas", column: "StatusId");

        migrationBuilder.CreateIndex(
            name: "IX_IdeaStatusTranslations_IdeaStatusId_Language",
            table: "IdeaStatusTranslations",
            columns: _ideaStatusTranslationIndexColumns,
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "IdeaDomainTranslations");

        migrationBuilder.DropTable(name: "IdeaIdeaTag");

        migrationBuilder.DropTable(name: "IdeaKeyword");

        migrationBuilder.DropTable(name: "IdeaNotes");

        migrationBuilder.DropTable(name: "IdeaPriorityTranslations");

        migrationBuilder.DropTable(name: "IdeaStatusTranslations");

        migrationBuilder.DropTable(name: "IdeaTags");

        migrationBuilder.DropTable(name: "Ideas");

        migrationBuilder.DropTable(name: "IdeaDomains");

        migrationBuilder.DropTable(name: "IdeaPriorities");

        migrationBuilder.DropTable(name: "IdeaStatuses");
    }
}
