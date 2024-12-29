using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagWarnToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWarningComment",
                table: "CompanyComments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWarningComment",
                table: "CommentOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LogUsersVisitToCompanyComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeVisit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyOwnerCommentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogUsersVisitToCompanyComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogUsersVisitToOrderComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTimeVisit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderOwnerCommentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogUsersVisitToOrderComments", x => x.Id);
                });
            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyComments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "CommentOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogUsersVisitToCompanyComments");

            migrationBuilder.DropTable(
                name: "LogUsersVisitToOrderComments");

            migrationBuilder.DropColumn(
                name: "IsWarningComment",
                table: "CompanyComments");

            migrationBuilder.DropColumn(
                name: "IsWarningComment",
                table: "CommentOrders");
            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "CommentOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyComments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
