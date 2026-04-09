using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class AddCompanyWebsiteAndRegion : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Region",
            table: "Companies",
            type: "character varying(255)",
            maxLength: 255,
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "Website",
            table: "Companies",
            type: "character varying(255)",
            maxLength: 255,
            nullable: true
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Region", table: "Companies");

        migrationBuilder.DropColumn(name: "Website", table: "Companies");
    }
}
