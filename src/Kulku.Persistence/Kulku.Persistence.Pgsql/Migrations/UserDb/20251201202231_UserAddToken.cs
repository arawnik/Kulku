using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kulku.Persistence.Pgsql.Migrations.UserDb;

/// <inheritdoc />
public partial class UserAddToken : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "UserToken",
            type: "character varying(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "UserToken",
            type: "character varying(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "ProviderKey",
            table: "UserLogin",
            type: "character varying(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "UserLogin",
            type: "character varying(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text"
        );

        migrationBuilder.AlterColumn<string>(
            name: "PhoneNumber",
            table: "User",
            type: "character varying(256)",
            maxLength: 256,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true
        );

        migrationBuilder.CreateTable(
            name: "AspNetUserPasskeys",
            columns: table => new
            {
                CredentialId = table.Column<byte[]>(
                    type: "bytea",
                    maxLength: 1024,
                    nullable: false
                ),
                UserId = table.Column<string>(type: "text", nullable: false),
                Data = table.Column<string>(type: "jsonb", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserPasskeys", x => x.CredentialId);
                table.ForeignKey(
                    name: "FK_AspNetUserPasskeys_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserPasskeys_UserId",
            table: "AspNetUserPasskeys",
            column: "UserId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "AspNetUserPasskeys");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "UserToken",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128
        );

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "UserToken",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128
        );

        migrationBuilder.AlterColumn<string>(
            name: "ProviderKey",
            table: "UserLogin",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128
        );

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "UserLogin",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128
        );

        migrationBuilder.AlterColumn<string>(
            name: "PhoneNumber",
            table: "User",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(256)",
            oldMaxLength: 256,
            oldNullable: true
        );
    }
}
