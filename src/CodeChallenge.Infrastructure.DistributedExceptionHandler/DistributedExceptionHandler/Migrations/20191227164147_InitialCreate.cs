using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistributedExceptionHandler.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionModels",
                columns: table => new
                {
                    ExceptionGuid = table.Column<Guid>(nullable: false),
                    Origin = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    ExceptionDate = table.Column<DateTime>(nullable: false),
                    ExceptionConsumedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionModels", x => x.ExceptionGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionModels");
        }
    }
}
