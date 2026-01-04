using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class ReworkEnums : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Re-map legacy keyword type codes to enum names.
        // Old: "SK", "LA", "TE"
        // New: "Skill", "Language", "Technology"
        migrationBuilder.Sql(
            """
    UPDATE "Keywords"
    SET "Type" = CASE UPPER(TRIM("Type"))
        WHEN 'SK' THEN 'Skill'
        WHEN 'LA' THEN 'Language'
        WHEN 'TE' THEN 'Technology'
        ELSE "Type"
    END
    WHERE UPPER(TRIM("Type")) IN ('SK', 'LA', 'TE');
"""
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ProjectTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ProficiencyTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "KeywordTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "IntroductionTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "InstitutionTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ExperienceTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "EducationTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Subject",
            table: "ContactRequests",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "ContactRequests",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Message",
            table: "ContactRequests",
            type: "character varying(2000)",
            maxLength: 2000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "ContactRequests",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ContactRequests",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "CompanyTranslations",
            type: "character varying(2)",
            unicode: false,
            maxLength: 2,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Reverse re-map enum names back to legacy codes.
        migrationBuilder.Sql(
            """
    UPDATE "Keywords"
    SET "Type" = CASE
        WHEN "Type" = 'Skill' THEN 'SK'
        WHEN "Type" = 'Language' THEN 'LA'
        WHEN "Type" = 'Technology' THEN 'TE'
        ELSE "Type"
    END
    WHERE "Type" IN ('Skill', 'Language', 'Technology');
"""
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ProjectTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ProficiencyTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "KeywordTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "IntroductionTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "InstitutionTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "ExperienceTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "EducationTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );

        migrationBuilder.AlterColumn<string>(
            name: "Subject",
            table: "ContactRequests",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255
        );

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "ContactRequests",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255
        );

        migrationBuilder.AlterColumn<string>(
            name: "Message",
            table: "ContactRequests",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2000)",
            oldMaxLength: 2000
        );

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "ContactRequests",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ContactRequests",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<string>(
            name: "Language",
            table: "CompanyTranslations",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(2)",
            oldUnicode: false,
            oldMaxLength: 2
        );
    }
}
