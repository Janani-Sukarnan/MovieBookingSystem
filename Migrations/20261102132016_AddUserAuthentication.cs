using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBookingSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuthentication : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Users table first
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    Name = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false),

                    Email = table.Column<string>(
                        type: "nvarchar(150)",
                        maxLength: 150,
                        nullable: false),

                    PasswordHash = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false),

                    Role = table.Column<string>(
                        type: "nvarchar(30)",
                        maxLength: 30,
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            // 2. Create a placeholder user for existing bookings
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
            "UserId", "Name", "Email", "PasswordHash", "Role"
                },
                values: new object[]
                {
            1,
            "Existing Booking Customer",
            "legacy-bookings@moviebooking.local",
            "LEGACY_ACCOUNT_NO_LOGIN",
            "Customer"
                });

            // 3. Add UserId as nullable temporarily
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bookings",
                type: "int",
                nullable: true);

            // 4. Assign existing bookings to the placeholder user
            migrationBuilder.Sql(
                "UPDATE Bookings SET UserId = 1 WHERE UserId IS NULL");

            // 5. Make UserId non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 6. Create index
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            // 7. Add foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");
        }
    }
}
