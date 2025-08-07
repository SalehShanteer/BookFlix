using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookTriggerForUpdateTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TRIGGER TRG_UpdateBookTimestamp
            ON Books
            AFTER UPDATE
            AS 
            BEGIN
            	SET NOCOUNT ON;
            	UPDATE Books
            	SET UpdatedAt = GETDATE()
            	FROM Books b
            	INNER JOIN inserted i ON b.Id = i.Id
            END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER TRG_UpdateBookTimestamp;");
        }
    }
}
