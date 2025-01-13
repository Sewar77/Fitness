using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFitnessLife.Migrations
{
    /// <inheritdoc />
    public partial class AddDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FITNESS");

            migrationBuilder.CreateTable(
                name: "BANK",
                schema: "FITNESS",
                columns: table => new
                {
                    BANKID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CARD = table.Column<decimal>(type: "NUMBER", nullable: false),
                    BALANCE = table.Column<decimal>(type: "NUMBER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008077", x => x.BANKID);
                });

            migrationBuilder.CreateTable(
                name: "MEMBERSHIPPLANS",
                schema: "FITNESS",
                columns: table => new
                {
                    PLANID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLANNAME = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    DURATIONINMONTHS = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PRICE = table.Column<decimal>(type: "NUMBER", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: true),
                    CREATEDAT = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008090", x => x.PLANID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                schema: "FITNESS",
                columns: table => new
                {
                    ROLEID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ROLENAME = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008073", x => x.ROLEID);
                });

            migrationBuilder.CreateTable(
    name: "VISITOR",
    schema: "FITNESS",
    columns: table => new
    {
        VISITORID = table.Column<decimal>(type: "NUMBER", nullable: false)
            .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),  // Unique identifier for each visitor
        IP_ADDRESS = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true),  // Visitor's IP address
        VISITTIME = table.Column<DateTime>(type: "DATE", nullable: true, defaultValueSql: "SYSDATE"),  // Visit time, default to current time
        SESSIONID = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true),  // Session ID
        HASREGISTERED = table.Column<bool>(type: "NUMBER(1)", precision: 1, nullable: true, defaultValueSql: "0"),  // Flag for registration status (0 = not registered, 1 = registered)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_VISITOR", x => x.VISITORID);  // Primary key for VISITORID
    });


            migrationBuilder.CreateTable(
                name: "USERS",
                schema: "FITNESS",
                columns: table => new
                {
                    USERID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USERNAME = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: false),
                    PASSWORD = table.Column<string>(type: "VARCHAR2(255)", unicode: false, maxLength: 255, nullable: false),
                    ROLEID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    FNAME = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true),
                    LNAME = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true),
                    EMAIL = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: true),
                    PHONENUMBER = table.Column<string>(type: "VARCHAR2(15)", unicode: false, maxLength: 15, nullable: true),
                    CREATEDAT = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    GENDER = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    BIRTHDATE = table.Column<DateTime>(type: "DATE", nullable: true),
                    ISACTIVE = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1) , // Adding the IsActive column
                    IMAGEPATH = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: true),
                    STATUS = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008082", x => x.USERID);
                    table.ForeignKey(
                        name: "SYS_C008085",
                        column: x => x.ROLEID,
                        principalSchema: "FITNESS",
                        principalTable: "ROLES",
                        principalColumn: "ROLEID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
    name: "StaticPages",
    schema: "FITNESS",
    columns: table => new
    {
        PageID = table.Column<decimal>(type: "NUMBER", nullable: false)
            .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
        PageName = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: false),
        PageContent = table.Column<string>(type: "CLOB", nullable: false),
        LastUpdated = table.Column<DateTime>(type: "TIMESTAMP(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
        IsActive = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1)
            .Annotation("Oracle:Check", "IsActive IN (0, 1)")  // Add the check constraint for IsActive
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_StaticPages", x => x.PageID);
    });


            migrationBuilder.CreateTable(
                name: "FEEDBACKS",
                schema: "FITNESS",
                columns: table => new
                {
                    FEEDBACKID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    FEEDBACKTEXT = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    SUBMITTEDAT = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    APPROVED = table.Column<bool>(type: "NUMBER(1)", precision: 1, nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008122", x => x.FEEDBACKID);
                    table.ForeignKey(
                        name: "SYS_C008123",
                        column: x => x.USERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SUBSCRIPTIONS",
                schema: "FITNESS",
                columns: table => new
                {
                    SUBSCRIPTIONID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    PLANID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    STARTDATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    ENDDATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    PAYMENTSTATUS = table.Column<string>(type: "VARCHAR2(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "'Pending' "),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(38)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008097", x => x.SUBSCRIPTIONID);
                    table.ForeignKey(
                        name: "SYS_C008098",
                        column: x => x.USERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SYS_C008099",
                        column: x => x.PLANID,
                        principalSchema: "FITNESS",
                        principalTable: "MEMBERSHIPPLANS",
                        principalColumn: "PLANID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAINERASSIGNMENTS",
                schema: "FITNESS",
                columns: table => new
                {
                    ASSIGNMENTID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TRAINERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    MEMBERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    ASSIGNEDAT = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008109", x => x.ASSIGNMENTID);
                    table.ForeignKey(
                        name: "SYS_C008110",
                        column: x => x.TRAINERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SYS_C008111",
                        column: x => x.MEMBERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WORKOUTS",
                schema: "FITNESS",
                columns: table => new
                {
                    WORKOUTID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TRAINERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    MEMBERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    GOALS = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: true),
                    CREATEDAT = table.Column<DateTime>(type: "TIMESTAMP(6)", precision: 6, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008103", x => x.WORKOUTID);
                    table.ForeignKey(
                        name: "SYS_C008104",
                        column: x => x.TRAINERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SYS_C008105",
                        column: x => x.MEMBERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INVOICES",
                schema: "FITNESS",
                columns: table => new
                {
                    INVOICEID = table.Column<decimal>(type: "NUMBER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SUBSCRIPTIONID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    USERID = table.Column<decimal>(type: "NUMBER(38)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    INVOICEDATE = table.Column<DateTime>(type: "DATE", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PDFPATH = table.Column<string>(type: "VARCHAR2(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SYS_C008116", x => x.INVOICEID);
                    table.ForeignKey(
                        name: "SYS_C008117",
                        column: x => x.SUBSCRIPTIONID,
                        principalSchema: "FITNESS",
                        principalTable: "SUBSCRIPTIONS",
                        principalColumn: "SUBSCRIPTIONID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SYS_C008118",
                        column: x => x.USERID,
                        principalSchema: "FITNESS",
                        principalTable: "USERS",
                        principalColumn: "USERID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FEEDBACKS_USERID",
                schema: "FITNESS",
                table: "FEEDBACKS",
                column: "USERID");

            migrationBuilder.CreateIndex(
                name: "IX_INVOICES_SUBSCRIPTIONID",
                schema: "FITNESS",
                table: "INVOICES",
                column: "SUBSCRIPTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_INVOICES_USERID",
                schema: "FITNESS",
                table: "INVOICES",
                column: "USERID");

            migrationBuilder.CreateIndex(
                name: "SYS_C008074",
                schema: "FITNESS",
                table: "ROLES",
                column: "ROLENAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTIONS_PLANID",
                schema: "FITNESS",
                table: "SUBSCRIPTIONS",
                column: "PLANID");

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTIONS_USERID",
                schema: "FITNESS",
                table: "SUBSCRIPTIONS",
                column: "USERID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAINERASSIGNMENTS_MEMBERID",
                schema: "FITNESS",
                table: "TRAINERASSIGNMENTS",
                column: "MEMBERID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAINERASSIGNMENTS_TRAINERID",
                schema: "FITNESS",
                table: "TRAINERASSIGNMENTS",
                column: "TRAINERID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_ROLEID",
                schema: "FITNESS",
                table: "USERS",
                column: "ROLEID");

            migrationBuilder.CreateIndex(
                name: "SYS_C008083",
                schema: "FITNESS",
                table: "USERS",
                column: "USERNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "SYS_C008084",
                schema: "FITNESS",
                table: "USERS",
                column: "EMAIL",
                unique: true,
                filter: "\"EMAIL\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WORKOUTS_MEMBERID",
                schema: "FITNESS",
                table: "WORKOUTS",
                column: "MEMBERID");

            migrationBuilder.CreateIndex(
                name: "IX_WORKOUTS_TRAINERID",
                schema: "FITNESS",
                table: "WORKOUTS",
                column: "TRAINERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANK",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "FEEDBACKS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "INVOICES",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "TRAINERASSIGNMENTS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "VISITOR",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "WORKOUTS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "SUBSCRIPTIONS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "USERS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "MEMBERSHIPPLANS",
                schema: "FITNESS");

            migrationBuilder.DropTable(
                name: "ROLES",
                schema: "FITNESS");
        }
    }
}
