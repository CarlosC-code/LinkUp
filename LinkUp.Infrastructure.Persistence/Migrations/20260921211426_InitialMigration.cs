using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkUp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amistades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserBId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SinceUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amistades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleshipAttacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    AttackerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TargetRow = table.Column<int>(type: "int", nullable: false),
                    TargetCol = table.Column<int>(type: "int", nullable: false),
                    IsHit = table.Column<bool>(type: "bit", nullable: false),
                    PerformedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleshipAttacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleshipGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OpponentUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentTurnUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    LastMoveAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WinnerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatorReady = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OpponentReady = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleshipGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleshipShips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    OwnerUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    StartRow = table.Column<int>(type: "int", nullable: false),
                    StartCol = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    PlacedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSunk = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleshipShips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PublishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes_Amistad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReceiverUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes_Amistad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Comentarios_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Publicaciones_PostId",
                        column: x => x.PostId,
                        principalTable: "Publicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reacciones_Publicaciones_PostId",
                        column: x => x.PostId,
                        principalTable: "Publicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amistades_UserAId",
                table: "Amistades",
                column: "UserAId");

            migrationBuilder.CreateIndex(
                name: "IX_Amistades_UserAId_UserBId",
                table: "Amistades",
                columns: new[] { "UserAId", "UserBId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amistades_UserBId",
                table: "Amistades",
                column: "UserBId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipAttacks_GameId_AttackerUserId",
                table: "BattleshipAttacks",
                columns: new[] { "GameId", "AttackerUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipAttacks_GameId_AttackerUserId_TargetRow_TargetCol",
                table: "BattleshipAttacks",
                columns: new[] { "GameId", "AttackerUserId", "TargetRow", "TargetCol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipGames_CreatorUserId",
                table: "BattleshipGames",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipGames_OpponentUserId",
                table: "BattleshipGames",
                column: "OpponentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipGames_Status",
                table: "BattleshipGames",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BattleshipShips_GameId_OwnerUserId",
                table: "BattleshipShips",
                columns: new[] { "GameId", "OwnerUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ParentCommentId",
                table: "Comentarios",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PostId_ParentCommentId",
                table: "Comentarios",
                columns: new[] { "PostId", "ParentCommentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UserId",
                table: "Comentarios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_PublishedAtUtc",
                table: "Publicaciones",
                column: "PublishedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_UserId",
                table: "Publicaciones",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_PostId_UserId",
                table: "Reacciones",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_Amistad_ReceiverUserId",
                table: "Solicitudes_Amistad",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_Amistad_SenderUserId",
                table: "Solicitudes_Amistad",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_Amistad_SenderUserId_ReceiverUserId_Status",
                table: "Solicitudes_Amistad",
                columns: new[] { "SenderUserId", "ReceiverUserId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amistades");

            migrationBuilder.DropTable(
                name: "BattleshipAttacks");

            migrationBuilder.DropTable(
                name: "BattleshipGames");

            migrationBuilder.DropTable(
                name: "BattleshipShips");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Reacciones");

            migrationBuilder.DropTable(
                name: "Solicitudes_Amistad");

            migrationBuilder.DropTable(
                name: "Publicaciones");
        }
    }
}
