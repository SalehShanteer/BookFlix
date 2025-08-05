using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addbookgenrebookauthorentitiesandaddbookcontrollerwithbookDtovalidationandfixsomebugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TextFilePath",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
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
                name: "BookGenres",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald" },
                    { 2, "Harper Lee" },
                    { 3, "George Orwell" },
                    { 4, "Jane Austen" },
                    { 5, "J.R.R. Tolkien" },
                    { 6, "Frank Herbert" },
                    { 7, "Yuval Noah Harari" },
                    { 8, "Dan Brown" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AverageRating", "CoverImageUrl", "CreatedAt", "Description", "FileLocation", "ISBN", "IsAvailable", "PageCount", "PublicationDate", "Publisher", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 4.20m, "https://example.com/great-gatsby.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A novel set in the Roaring Twenties.", null, "9780743273565", true, 180, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scribner", "The Great Gatsby", null },
                    { 2, 4.30m, "https://example.com/to-kill-a-mockingbird.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A novel about racial injustice in the Deep South.", null, "9780061120084", true, 281, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.B. Lippincott & Co.", "To Kill a Mockingbird", null },
                    { 3, 4.40m, "https://example.com/1984.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A dystopian novel about totalitarianism.", null, "9780451524935", true, 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secker & Warburg", "1984", null },
                    { 4, 4.25m, "https://example.com/pride-and-prejudice.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A romantic novel about love and social class.", null, "9780141439518", true, 432, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Penguin Classics", "Pride and Prejudice", null },
                    { 5, 4.27m, "https://example.com/the-hobbit.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A fantasy adventure about Bilbo Baggins.", null, "9780547928227", true, 310, new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Houghton Mifflin", "The Hobbit", null },
                    { 6, 4.21m, "https://example.com/dune.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A science fiction epic about a desert planet.", null, "9780441172719", true, 412, new DateTime(1965, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ace Books", "Dune", null },
                    { 7, 4.38m, "https://example.com/sapiens.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A nonfiction exploration of human history.", null, "9780062316097", true, 443, new DateTime(2014, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harper", "Sapiens: A Brief History of Humankind", null },
                    { 8, 3.85m, "https://example.com/da-vinci-code.jpg", new DateTime(2025, 8, 5, 10, 0, 0, 0, DateTimeKind.Local), "A thriller involving a religious conspiracy.", null, "9780307277671", true, 454, new DateTime(2003, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doubleday", "The Da Vinci Code", null }
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
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 3 },
                    { 4, 7 },
                    { 4, 8 },
                    { 5, 4 },
                    { 5, 19 },
                    { 6, 3 },
                    { 7, 2 },
                    { 7, 14 },
                    { 8, 5 },
                    { 8, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextFilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    BooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");
        }
    }
}
