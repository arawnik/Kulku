using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class ProjectAdd : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Proficiencies",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Scale = table.Column<int>(type: "integer", nullable: false),
                Order = table.Column<int>(type: "integer", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Proficiencies", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Url = table.Column<string>(type: "text", nullable: false),
                Order = table.Column<int>(type: "integer", nullable: false),
                ImageUrl = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Keywords",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                Type = table.Column<string>(type: "text", nullable: false),
                Order = table.Column<int>(type: "integer", nullable: false),
                Display = table.Column<bool>(type: "boolean", nullable: false),
                ProficiencyId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Keywords", x => x.Id);
                table.ForeignKey(
                    name: "FK_Keywords_Proficiencies_ProficiencyId",
                    column: x => x.ProficiencyId,
                    principalTable: "Proficiencies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "ProficiencyTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                ProficiencyId = table.Column<Guid>(type: "uuid", nullable: false),
                Language = table.Column<string>(type: "text", nullable: false),
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
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProficiencyTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProficiencyTranslations_Proficiencies_ProficiencyId",
                    column: x => x.ProficiencyId,
                    principalTable: "Proficiencies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "ProjectTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                Language = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Info = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProjectTranslations_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "KeywordTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                KeywordId = table.Column<Guid>(type: "uuid", nullable: false),
                Language = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KeywordTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_KeywordTranslations_Keywords_KeywordId",
                    column: x => x.KeywordId,
                    principalTable: "Keywords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "ProjectKeyword",
            columns: table => new
            {
                ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                KeywordId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectKeyword", x => new { x.ProjectId, x.KeywordId });
                table.ForeignKey(
                    name: "FK_ProjectKeyword_Keywords_KeywordId",
                    column: x => x.KeywordId,
                    principalTable: "Keywords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_ProjectKeyword_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Keywords_ProficiencyId",
            table: "Keywords",
            column: "ProficiencyId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_KeywordTranslations_KeywordId_Language",
            table: "KeywordTranslations",
            columns: ["KeywordId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_ProficiencyTranslations_ProficiencyId_Language",
            table: "ProficiencyTranslations",
            columns: ["ProficiencyId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_ProjectKeyword_KeywordId",
            table: "ProjectKeyword",
            column: "KeywordId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_ProjectTranslations_ProjectId_Language",
            table: "ProjectTranslations",
            columns: ["ProjectId", "Language"],
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "KeywordTranslations");

        migrationBuilder.DropTable(name: "ProficiencyTranslations");

        migrationBuilder.DropTable(name: "ProjectKeyword");

        migrationBuilder.DropTable(name: "ProjectTranslations");

        migrationBuilder.DropTable(name: "Keywords");

        migrationBuilder.DropTable(name: "Projects");

        migrationBuilder.DropTable(name: "Proficiencies");
    }
}
