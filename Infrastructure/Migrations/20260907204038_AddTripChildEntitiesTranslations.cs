using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripChildEntitiesTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "TripTranslations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TripExcludeTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TripExcludeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripExcludeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripExcludeTranslations_TripExcludes_TripExcludeId",
                        column: x => x.TripExcludeId,
                        principalTable: "TripExcludes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripHighlightTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TripHighlightId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripHighlightTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripHighlightTranslations_TripHighlights_TripHighlightId",
                        column: x => x.TripHighlightId,
                        principalTable: "TripHighlights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripIncludeTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TripIncludeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripIncludeTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripIncludeTranslations_TripIncludes_TripIncludeId",
                        column: x => x.TripIncludeId,
                        principalTable: "TripIncludes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripItineraryItemTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TripItineraryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripItineraryItemTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripItineraryItemTranslations_TripItineraryItems_TripItineraryItemId",
                        column: x => x.TripItineraryItemId,
                        principalTable: "TripItineraryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripWhatToBringTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TripWhatToBringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripWhatToBringTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripWhatToBringTranslations_TripWhatToBrings_TripWhatToBringId",
                        column: x => x.TripWhatToBringId,
                        principalTable: "TripWhatToBrings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripExcludeTranslations_TripExcludeId_Language",
                table: "TripExcludeTranslations",
                columns: new[] { "TripExcludeId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripHighlightTranslations_TripHighlightId_Language",
                table: "TripHighlightTranslations",
                columns: new[] { "TripHighlightId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripIncludeTranslations_TripIncludeId_Language",
                table: "TripIncludeTranslations",
                columns: new[] { "TripIncludeId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripItineraryItemTranslations_TripItineraryItemId_Language",
                table: "TripItineraryItemTranslations",
                columns: new[] { "TripItineraryItemId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripWhatToBringTranslations_TripWhatToBringId_Language",
                table: "TripWhatToBringTranslations",
                columns: new[] { "TripWhatToBringId", "Language" },
                unique: true);

            migrationBuilder.Sql(
                @"INSERT INTO [TripItineraryItemTranslations] ([Id], [Language], [Title], [Description], [TripItineraryItemId])
SELECT NEWID(), 0, [Title], [Description], [Id]
FROM [TripItineraryItems]
WHERE [Title] IS NOT NULL;");

            migrationBuilder.Sql(
                @"INSERT INTO [TripIncludeTranslations] ([Id], [Language], [Description], [TripIncludeId])
SELECT NEWID(), 0, [Description], [Id]
FROM [TripIncludes]
WHERE [Description] IS NOT NULL;");

            migrationBuilder.Sql(
                @"INSERT INTO [TripExcludeTranslations] ([Id], [Language], [Description], [TripExcludeId])
SELECT NEWID(), 0, [Description], [Id]
FROM [TripExcludes]
WHERE [Description] IS NOT NULL;");

            migrationBuilder.Sql(
                @"INSERT INTO [TripHighlightTranslations] ([Id], [Language], [Description], [TripHighlightId])
SELECT NEWID(), 0, [Description], [Id]
FROM [TripHighlights]
WHERE [Description] IS NOT NULL;");

            migrationBuilder.Sql(
                @"INSERT INTO [TripWhatToBringTranslations] ([Id], [Language], [Description], [TripWhatToBringId])
SELECT NEWID(), 0, [Description], [Id]
FROM [TripWhatToBrings]
WHERE [Description] IS NOT NULL;");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TripWhatToBrings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TripItineraryItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TripItineraryItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TripIncludes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TripHighlights");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TripExcludes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TripWhatToBrings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TripItineraryItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TripItineraryItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TripIncludes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TripHighlights",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TripExcludes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                @"UPDATE it
SET [Title] = t.[Title], [Description] = t.[Description]
FROM [TripItineraryItems] it
INNER JOIN [TripItineraryItemTranslations] t ON t.[TripItineraryItemId] = it.[Id] AND t.[Language] = 0;");

            migrationBuilder.Sql(
                @"UPDATE i
SET [Description] = t.[Description]
FROM [TripIncludes] i
INNER JOIN [TripIncludeTranslations] t ON t.[TripIncludeId] = i.[Id] AND t.[Language] = 0;");

            migrationBuilder.Sql(
                @"UPDATE e
SET [Description] = t.[Description]
FROM [TripExcludes] e
INNER JOIN [TripExcludeTranslations] t ON t.[TripExcludeId] = e.[Id] AND t.[Language] = 0;");

            migrationBuilder.Sql(
                @"UPDATE h
SET [Description] = t.[Description]
FROM [TripHighlights] h
INNER JOIN [TripHighlightTranslations] t ON t.[TripHighlightId] = h.[Id] AND t.[Language] = 0;");

            migrationBuilder.Sql(
                @"UPDATE w
SET [Description] = t.[Description]
FROM [TripWhatToBrings] w
INNER JOIN [TripWhatToBringTranslations] t ON t.[TripWhatToBringId] = w.[Id] AND t.[Language] = 0;");

            migrationBuilder.DropTable(
                name: "TripExcludeTranslations");

            migrationBuilder.DropTable(
                name: "TripHighlightTranslations");

            migrationBuilder.DropTable(
                name: "TripIncludeTranslations");

            migrationBuilder.DropTable(
                name: "TripItineraryItemTranslations");

            migrationBuilder.DropTable(
                name: "TripWhatToBringTranslations");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "TripTranslations");
        }
    }
}