using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedGSTApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRolePermissionManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(name: "LastLoginDate", table: "AspNetUsers", type: "datetime2", nullable: true);
            migrationBuilder.AddColumn<string>(name: "ProfilePhoto", table: "AspNetUsers", type: "nvarchar(300)", maxLength: 300, nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "UpdatedDate", table: "AspNetUsers", type: "datetime2", nullable: true);

            migrationBuilder.AddColumn<DateTime>(name: "CreatedDate", table: "AspNetRoles", type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()");
            migrationBuilder.AddColumn<bool>(name: "IsActive", table: "AspNetRoles", type: "bit", nullable: false, defaultValue: true);
            migrationBuilder.AddColumn<DateTime>(name: "UpdatedDate", table: "AspNetRoles", type: "datetime2", nullable: true);

            migrationBuilder.AddColumn<string>(name: "Description", table: "AuditLogs", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "IpAddress", table: "AuditLogs", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "ModuleName", table: "AuditLogs", type: "nvarchar(max)", nullable: true);

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanPrint = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false),
                    CanEmail = table.Column<bool>(type: "bit", nullable: false),
                    CanGenerate = table.Column<bool>(type: "bit", nullable: false),
                    CanApprove = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(name: "FK_RolePermissions_AspNetRoles_RoleId", column: x => x.RoleId, principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_RolePermissions_RoleId_ModuleName", table: "RolePermissions", columns: new[] { "RoleId", "ModuleName" }, unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "RolePermissions");
            migrationBuilder.DropColumn(name: "LastLoginDate", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "ProfilePhoto", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "UpdatedDate", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "CreatedDate", table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "IsActive", table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "UpdatedDate", table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "Description", table: "AuditLogs");
            migrationBuilder.DropColumn(name: "IpAddress", table: "AuditLogs");
            migrationBuilder.DropColumn(name: "ModuleName", table: "AuditLogs");
        }
    }
}
