using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.AppDb;

/// <inheritdoc />
public partial class AddContactRequestStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "ContactRequests",
            type: "text",
            nullable: false,
            defaultValue: "New"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Status", table: "ContactRequests");
    }
}
