using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class AddCover : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Institutions",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Institutions", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Introductions",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                AvatarUrl = table.Column<string>(type: "text", nullable: false),
                SmallAvatarUrl = table.Column<string>(type: "text", nullable: false),
                PubDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Introductions", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "CompanyTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_CompanyTranslations_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "Experiences",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                EndDate = table.Column<DateOnly>(type: "date", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Experiences", x => x.Id);
                table.ForeignKey(
                    name: "FK_Experiences_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "Educations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                EndDate = table.Column<DateOnly>(type: "date", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Educations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Educations_Institutions_InstitutionId",
                    column: x => x.InstitutionId,
                    principalTable: "Institutions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "InstitutionTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Department = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InstitutionTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_InstitutionTranslations_Institutions_InstitutionId",
                    column: x => x.InstitutionId,
                    principalTable: "Institutions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "IntroductionTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                IntroductionId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Content = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Tagline = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Language = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IntroductionTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_IntroductionTranslations_Introductions_IntroductionId",
                    column: x => x.IntroductionId,
                    principalTable: "Introductions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "ExperienceKeyword",
            columns: table => new
            {
                ExperienceId = table.Column<Guid>(type: "uuid", nullable: false),
                KeywordsId = table.Column<Guid>(type: "uuid", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExperienceKeyword", x => new { x.ExperienceId, x.KeywordsId });
                table.ForeignKey(
                    name: "FK_ExperienceKeyword_Experiences_ExperienceId",
                    column: x => x.ExperienceId,
                    principalTable: "Experiences",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_ExperienceKeyword_Keywords_KeywordsId",
                    column: x => x.KeywordsId,
                    principalTable: "Keywords",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "ExperienceTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                ExperienceId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExperienceTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExperienceTranslations_Experiences_ExperienceId",
                    column: x => x.ExperienceId,
                    principalTable: "Experiences",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "EducationTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uuid",
                    nullable: false,
                    defaultValueSql: "gen_random_uuid()"
                ),
                EducationId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(
                    type: "character varying(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "character varying(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "text", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EducationTranslations", x => x.Id);
                table.ForeignKey(
                    name: "FK_EducationTranslations_Educations_EducationId",
                    column: x => x.EducationId,
                    principalTable: "Educations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyTranslations_CompanyId_Language",
            table: "CompanyTranslations",
            columns: ["CompanyId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_Educations_InstitutionId",
            table: "Educations",
            column: "InstitutionId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_EducationTranslations_EducationId_Language",
            table: "EducationTranslations",
            columns: ["EducationId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_ExperienceKeyword_KeywordsId",
            table: "ExperienceKeyword",
            column: "KeywordsId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_Experiences_CompanyId",
            table: "Experiences",
            column: "CompanyId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_ExperienceTranslations_ExperienceId_Language",
            table: "ExperienceTranslations",
            columns: ["ExperienceId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionTranslations_InstitutionId_Language",
            table: "InstitutionTranslations",
            columns: ["InstitutionId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_IntroductionTranslations_IntroductionId_Language",
            table: "IntroductionTranslations",
            columns: ["IntroductionId", "Language"],
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "CompanyTranslations");

        migrationBuilder.DropTable(name: "EducationTranslations");

        migrationBuilder.DropTable(name: "ExperienceKeyword");

        migrationBuilder.DropTable(name: "ExperienceTranslations");

        migrationBuilder.DropTable(name: "InstitutionTranslations");

        migrationBuilder.DropTable(name: "IntroductionTranslations");

        migrationBuilder.DropTable(name: "Educations");

        migrationBuilder.DropTable(name: "Experiences");

        migrationBuilder.DropTable(name: "Introductions");

        migrationBuilder.DropTable(name: "Institutions");

        migrationBuilder.DropTable(name: "Companies");
    }
}
