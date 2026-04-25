using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "User"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "F. Scott Fitzgerald" },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "Harper Lee" },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "George Orwell" },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "Jane Austen" },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), "J.R.R. Tolkien" },
                    { new Guid("a0000000-0000-0000-0000-000000000006"), "Frank Herbert" },
                    { new Guid("a0000000-0000-0000-0000-000000000007"), "Yuval Noah Harari" },
                    { new Guid("a0000000-0000-0000-0000-000000000008"), "Dan Brown" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AverageRating", "CoverImageUrl", "CreatedAt", "Description", "FileLocation", "ISBN", "IsAvailable", "PageCount", "PublicationDate", "Publisher", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), 4.20m, "https://example.com/great-gatsby.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A novel set in the Roaring Twenties.", null, "9780743273565", true, 180, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scribner", "The Great Gatsby", null },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), 4.30m, "https://example.com/to-kill-a-mockingbird.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A novel about racial injustice in the Deep South.", null, "9780061120084", true, 281, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.B. Lippincott & Co.", "To Kill a Mockingbird", null },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), 4.40m, "https://example.com/1984.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A dystopian novel about totalitarianism.", null, "9780451524935", true, 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secker & Warburg", "1984", null },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), 4.25m, "https://example.com/pride-and-prejudice.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A romantic novel about love and social class.", null, "9780141439518", true, 432, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Penguin Classics", "Pride and Prejudice", null },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), 4.27m, "https://example.com/the-hobbit.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A fantasy adventure about Bilbo Baggins.", null, "9780547928227", true, 310, new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Houghton Mifflin", "The Hobbit", null },
                    { new Guid("b0000000-0000-0000-0000-000000000006"), 4.21m, "https://example.com/dune.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A science fiction epic about a desert planet.", null, "9780441172719", true, 412, new DateTime(1965, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ace Books", "Dune", null },
                    { new Guid("b0000000-0000-0000-0000-000000000007"), 4.38m, "https://example.com/sapiens.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A nonfiction exploration of human history.", null, "9780062316097", true, 443, new DateTime(2014, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harper", "Sapiens: A Brief History of Humankind", null },
                    { new Guid("b0000000-0000-0000-0000-000000000008"), 3.85m, "https://example.com/da-vinci-code.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Utc), "A thriller involving a religious conspiracy.", null, "9780307277671", true, 454, new DateTime(2003, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doubleday", "The Da Vinci Code", null }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Nonfiction" },
                    { 3, "Science Fiction" },
                    { 4, "Fantasy" },
                    { 5, "Mystery" },
                    { 6, "Thriller" },
                    { 7, "Romance" },
                    { 8, "Historical Fiction" },
                    { 9, "Biography" },
                    { 10, "Autobiography" },
                    { 11, "Self-Help" },
                    { 12, "Business" },
                    { 13, "Science" },
                    { 14, "History" },
                    { 15, "Young Adult" },
                    { 16, "Children" },
                    { 17, "Poetry" },
                    { 18, "Horror" },
                    { 19, "Adventure" },
                    { 20, "Crime" },
                    { 21, "Literary Criticism" },
                    { 22, "Cooking" },
                    { 23, "Travel" },
                    { 24, "Philosophy" },
                    { 25, "Religion" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000002") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("b0000000-0000-0000-0000-000000000003") },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("b0000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("b0000000-0000-0000-0000-000000000005") },
                    { new Guid("a0000000-0000-0000-0000-000000000006"), new Guid("b0000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000007"), new Guid("b0000000-0000-0000-0000-000000000007") },
                    { new Guid("a0000000-0000-0000-0000-000000000008"), new Guid("b0000000-0000-0000-0000-000000000008") }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), 1 },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), 3 },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), 7 },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), 8 },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), 4 },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), 19 },
                    { new Guid("b0000000-0000-0000-0000-000000000006"), 3 },
                    { new Guid("b0000000-0000-0000-0000-000000000007"), 2 },
                    { new Guid("b0000000-0000-0000-0000-000000000007"), 14 },
                    { new Guid("b0000000-0000-0000-0000-000000000008"), 5 },
                    { new Guid("b0000000-0000-0000-0000-000000000008"), 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name",
                table: "Authors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true,
                filter: "[ISBN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId",
                table: "UserLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
