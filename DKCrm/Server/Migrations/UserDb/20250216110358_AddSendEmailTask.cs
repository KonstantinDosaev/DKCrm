using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCrm.Server.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddSendEmailTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SendEmailTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Emails = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumbers = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    AttachmentOne = table.Column<byte[]>(type: "bytea", nullable: true),
                    AttachmentTwo = table.Column<byte[]>(type: "bytea", nullable: true),
                    SendByUser = table.Column<bool>(type: "boolean", nullable: false),
                    PrivetTask = table.Column<bool>(type: "boolean", nullable: false),
                    DateTimeCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateTimeInit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserCreatorId = table.Column<string>(type: "character varying(60)", maxLength: 30, nullable: true),
                    UseCreatorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendEmailTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendEmailTasks_AspNetUsers_UseCreatorId",
                        column: x => x.UseCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SendEmailTasks_UseCreatorId",
                table: "SendEmailTasks",
                column: "UseCreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SendEmailTasks");
        }
    }
}
