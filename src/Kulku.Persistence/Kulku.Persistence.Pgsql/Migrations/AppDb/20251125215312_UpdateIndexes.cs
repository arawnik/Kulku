using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class UpdateIndexes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_IntroductionTranslations_IntroductionId_Language",
            table: "IntroductionTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_InstitutionTranslations_InstitutionId_Language",
            table: "InstitutionTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_ExperienceTranslations_ExperienceId_Language",
            table: "ExperienceTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_EducationTranslations_EducationId_Language",
            table: "EducationTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_CompanyTranslations_CompanyId_Language",
            table: "CompanyTranslations"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IntroductionTranslations_IntroductionId",
            table: "IntroductionTranslations",
            column: "IntroductionId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IntroductionTranslations_Language_IntroductionId",
            table: "IntroductionTranslations",
            columns: ["Language", "IntroductionId"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_Introductions_PubDate",
            table: "Introductions",
            column: "PubDate"
        );

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionTranslations_InstitutionId",
            table: "InstitutionTranslations",
            column: "InstitutionId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionTranslations_Language_InstitutionId",
            table: "InstitutionTranslations",
            columns: ["Language", "InstitutionId"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_ExperienceTranslations_ExperienceId",
            table: "ExperienceTranslations",
            column: "ExperienceId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_ExperienceTranslations_Language_ExperienceId",
            table: "ExperienceTranslations",
            columns: ["Language", "ExperienceId"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_Experiences_EndDate",
            table: "Experiences",
            column: "EndDate"
        );

        migrationBuilder.CreateIndex(
            name: "IX_EducationTranslations_EducationId",
            table: "EducationTranslations",
            column: "EducationId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_EducationTranslations_Language_EducationId",
            table: "EducationTranslations",
            columns: ["Language", "EducationId"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_Educations_EndDate",
            table: "Educations",
            column: "EndDate"
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyTranslations_CompanyId",
            table: "CompanyTranslations",
            column: "CompanyId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyTranslations_Language_CompanyId",
            table: "CompanyTranslations",
            columns: ["Language", "CompanyId"],
            unique: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_IntroductionTranslations_IntroductionId",
            table: "IntroductionTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_IntroductionTranslations_Language_IntroductionId",
            table: "IntroductionTranslations"
        );

        migrationBuilder.DropIndex(name: "IX_Introductions_PubDate", table: "Introductions");

        migrationBuilder.DropIndex(
            name: "IX_InstitutionTranslations_InstitutionId",
            table: "InstitutionTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_InstitutionTranslations_Language_InstitutionId",
            table: "InstitutionTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_ExperienceTranslations_ExperienceId",
            table: "ExperienceTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_ExperienceTranslations_Language_ExperienceId",
            table: "ExperienceTranslations"
        );

        migrationBuilder.DropIndex(name: "IX_Experiences_EndDate", table: "Experiences");

        migrationBuilder.DropIndex(
            name: "IX_EducationTranslations_EducationId",
            table: "EducationTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_EducationTranslations_Language_EducationId",
            table: "EducationTranslations"
        );

        migrationBuilder.DropIndex(name: "IX_Educations_EndDate", table: "Educations");

        migrationBuilder.DropIndex(
            name: "IX_CompanyTranslations_CompanyId",
            table: "CompanyTranslations"
        );

        migrationBuilder.DropIndex(
            name: "IX_CompanyTranslations_Language_CompanyId",
            table: "CompanyTranslations"
        );

        migrationBuilder.CreateIndex(
            name: "IX_IntroductionTranslations_IntroductionId_Language",
            table: "IntroductionTranslations",
            columns: ["IntroductionId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_InstitutionTranslations_InstitutionId_Language",
            table: "InstitutionTranslations",
            columns: ["InstitutionId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_ExperienceTranslations_ExperienceId_Language",
            table: "ExperienceTranslations",
            columns: ["ExperienceId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_EducationTranslations_EducationId_Language",
            table: "EducationTranslations",
            columns: ["EducationId", "Language"],
            unique: true
        );

        migrationBuilder.CreateIndex(
            name: "IX_CompanyTranslations_CompanyId_Language",
            table: "CompanyTranslations",
            columns: ["CompanyId", "Language"],
            unique: true
        );
    }
}
