using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.SqlServer.Migrations.AppDb;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "ContactRequests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactRequests", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Institutions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SmallAvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PubDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Introductions", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Proficiencies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Scale = table.Column<int>(type: "int", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "CompanyTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Department = table.Column<string>(
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: true
                ),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IntroductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Content = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Tagline = table.Column<string>(
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
            name: "Keywords",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                Display = table.Column<bool>(type: "bit", nullable: false),
                ProficiencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProficiencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: true
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
            name: "ExperienceTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ExperienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: false
                ),
                Description = table.Column<string>(
                    type: "nvarchar(2000)",
                    maxLength: 2000,
                    nullable: false
                ),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

        migrationBuilder.CreateTable(
            name: "ExperienceKeyword",
            columns: table => new
            {
                ExperienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeywordsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
            name: "KeywordTranslations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
        migrationBuilder.DropTable(name: "CompanyTranslations");

        migrationBuilder.DropTable(name: "ContactRequests");

        migrationBuilder.DropTable(name: "EducationTranslations");

        migrationBuilder.DropTable(name: "ExperienceKeyword");

        migrationBuilder.DropTable(name: "ExperienceTranslations");

        migrationBuilder.DropTable(name: "InstitutionTranslations");

        migrationBuilder.DropTable(name: "IntroductionTranslations");

        migrationBuilder.DropTable(name: "KeywordTranslations");

        migrationBuilder.DropTable(name: "ProficiencyTranslations");

        migrationBuilder.DropTable(name: "ProjectKeyword");

        migrationBuilder.DropTable(name: "ProjectTranslations");

        migrationBuilder.DropTable(name: "Educations");

        migrationBuilder.DropTable(name: "Experiences");

        migrationBuilder.DropTable(name: "Introductions");

        migrationBuilder.DropTable(name: "Keywords");

        migrationBuilder.DropTable(name: "Projects");

        migrationBuilder.DropTable(name: "Institutions");

        migrationBuilder.DropTable(name: "Companies");

        migrationBuilder.DropTable(name: "Proficiencies");
    }
}
