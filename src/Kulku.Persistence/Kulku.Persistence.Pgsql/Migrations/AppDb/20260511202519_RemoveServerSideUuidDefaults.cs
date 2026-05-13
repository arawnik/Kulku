using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class RemoveServerSideUuidDefaults : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ProjectTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Projects",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ProficiencyTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Proficiencies",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkInteractions",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkContacts",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkCategories",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "KeywordTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Keywords",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IntroductionTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Introductions",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "InstitutionTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Institutions",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaTags",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaStatusTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaStatuses",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Ideas",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaPriorityTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaPriorities",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaNotes",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaDomainTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaDomains",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ExperienceTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Experiences",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "EducationTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Educations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
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

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "CompanyTranslations",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "CompanyNetworkProfiles",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Companies",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldDefaultValueSql: "gen_random_uuid()"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ProjectTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Projects",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ProficiencyTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Proficiencies",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkInteractions",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkContacts",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "NetworkCategories",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "KeywordTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Keywords",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IntroductionTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Introductions",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "InstitutionTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Institutions",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaTags",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaStatusTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaStatuses",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Ideas",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaPriorityTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaPriorities",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaNotes",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaDomainTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "IdeaDomains",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "ExperienceTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Experiences",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "EducationTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Educations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
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

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "CompanyTranslations",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "CompanyNetworkProfiles",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "Companies",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()",
            oldClrType: typeof(Guid),
            oldType: "uuid"
        );
    }
}
