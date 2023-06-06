using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SurfWebApp.Data.Migrations
{
    public partial class surfwep2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurfLogs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogDate = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    HowLong = table.Column<TimeSpan>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurfLogs_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurfVideos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfVideos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FavSurfVideos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    SurfVideoID = table.Column<int>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavSurfVideos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FavSurfVideos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavSurfVideos_SurfVideos_SurfVideoID",
                        column: x => x.SurfVideoID,
                        principalTable: "SurfVideos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavSurfVideos_AppUserId",
                table: "FavSurfVideos",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavSurfVideos_SurfVideoID",
                table: "FavSurfVideos",
                column: "SurfVideoID");

            migrationBuilder.CreateIndex(
                name: "IX_SurfLogs_AppUserId",
                table: "SurfLogs",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavSurfVideos");

            migrationBuilder.DropTable(
                name: "SurfLogs");

            migrationBuilder.DropTable(
                name: "SurfVideos");
        }
    }
}
