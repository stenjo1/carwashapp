using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CarWashApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    Wallet = table.Column<double>(type: "float", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    TotalIncome = table.Column<double>(type: "float", nullable: true),
                    TotalLoss = table.Column<double>(type: "float", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarWashes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    OpeningHour = table.Column<long>(type: "bigint", nullable: false),
                    ClosingHour = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<Point>(type: "geography", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarWashes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarWashes_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarWashId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartHour = table.Column<int>(type: "int", nullable: false),
                    EndHour = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<long>(type: "bigint", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_CarWashes_CarWashId",
                        column: x => x.CarWashId,
                        principalTable: "CarWashes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revenues",
                columns: table => new
                {
                    CarWashId = table.Column<int>(type: "int", nullable: false),
                    CurrentValue = table.Column<double>(type: "float", nullable: false),
                    DailyIncome = table.Column<double>(type: "float", nullable: false),
                    WeeklyIncome = table.Column<double>(type: "float", nullable: false),
                    MonthlyIncome = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revenues", x => x.CarWashId);
                    table.ForeignKey(
                        name: "FK_Revenues_CarWashes_CarWashId",
                        column: x => x.CarWashId,
                        principalTable: "CarWashes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    CarWashId = table.Column<int>(type: "int", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => new { x.CarWashId, x.ServiceType });
                    table.ForeignKey(
                        name: "FK_Services_CarWashes_CarWashId",
                        column: x => x.CarWashId,
                        principalTable: "CarWashes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "Latitude", "Location", "LockoutEnabled", "LockoutEnd", "Longitude", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Wallet" },
                values: new object[,]
                {
                    { "0451bf9c-489b-454e-bdd4-7b4d84bec61c", 0, "33596d73-05ff-44f3-987b-d759c5f98f37", new DateTime(1999, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "simicmisa@gmail.com", false, "Mihailo", 0, "Simic", 44.772577474774678, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77257747477468 20.414438753254668)"), false, null, 20.414438753254668, "SIMICMISA@GMAIL.COM", "MISA1407", "AQAAAAEAACcQAAAAEEqnGnj9oe77aHprexCH7DGFf4sNKe99SV87dhzMLQ29vKeRYLoX0EPEJsKhhpoiyg==", "0621213321", false, "82a17111-d240-4b0b-b6ec-cca00c0c06a5", false, "misa1407", 1800.0 },
                    { "0d2c8a28-6268-4c63-b858-dc2e1118a776", 0, "27cfb499-4d6d-4594-92f3-76f91044e66d", new DateTime(1977, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "ivana.dimic@gmail.com", false, "Ivana", 1, "Dimic", 44.803230692950436, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.803230692950436 20.47186926038448)"), false, null, 20.47186926038448, "IVANA.DIMIC@GMAIL.COM", "IVANICADIM1", "AQAAAAEAACcQAAAAEANBxJ9IhyfBTvvpNRLK5YGrybX58IOquS0QWb55R7ou96OQwZOddwk45LjbPVhliA==", "0601252698", false, "3d3c2dad-222f-420d-a869-7e397720aff7", false, "ivanicadim1", 700.0 },
                    { "43be6494-af9d-4a17-9eed-5d1e03fe9c10", 0, "f05585d1-f999-4fa0-8934-3cf503530135", new DateTime(1992, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "jeca.zivkovic@gmail.com", false, "Jelena", 1, "Zivkovic", 44.769565513190734, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.769565513190734 20.411881545925098)"), false, null, 20.411881545925098, "JECA.ZIVKOVIC@GMAIL.COM", "JECAZIVKO33", "AQAAAAEAACcQAAAAEEniu7js0mv3cDcgBeY72csWch4IJ9pmgeh2fIdzjwTUHbHrpzRcXxAmSojXqrMxBA==", "0621213326", false, "9ef65379-9c30-41e4-bd71-39def1c14a10", false, "jecazivko33", 1000.0 },
                    { "449dc796-edaa-4afc-9ee2-1a347aba955e", 0, "c2e50b8b-a6ba-4cd7-8e9f-14fbe6ad6af1", new DateTime(1964, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "snezana.markovic@gmail.com", false, "Snezana", 1, "Markovic", 44.771380960895975, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.771380960895975 20.41100977069911)"), false, null, 20.41100977069911, "SNEZANA.MARKOVIC@GMAIL.COM", "SNESKIC120", "AQAAAAEAACcQAAAAEFR4gVkFQ4m1JDJQVpVKnQewQelepRIhfbb6t7rq2XsSrN0vbIiYeTKr3uKrYXlm3w==", "0621213323", false, "e966d1df-92a3-4f3e-bf30-3ea88bde3c5a", false, "sneskic120", 1200.0 },
                    { "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", 0, "c04e01c8-f115-4f82-a938-25d0c1ad5ba3", new DateTime(1986, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "milenabre@gmail.com", false, "Milena", 1, "Krkobabic", 44.80355175574644, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.80355175574644 20.47280907072259)"), false, null, 20.47280907072259, "MILENABRE@GMAIL.COM", "MILENSI556", "AQAAAAEAACcQAAAAEJgJsDIUD4qvNm+RTgK1JJwBgfZKI1dG5WccU7TRAZ19ovMYOu8wI4aKTIXxFuizeg==", "0643536982", false, "7a020cf7-fe3e-400a-aaa1-87efa1d27d19", false, "milensi556", 1600.0 },
                    { "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", 0, "75e8ee70-ad18-4a0a-b06d-1f9cc77e5139", new DateTime(1963, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "dragan.mar63@gmail.com", false, "Dragan", 0, "Markovic", 44.772102998099086, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.772102998099086 20.41440969408046)"), false, null, 20.414409694080462, "DRAGAN.MAR63@GMAIL.COM", "GAGISA631", "AQAAAAEAACcQAAAAEGFrSX7/qHzAnVlDubvknhEyId0tRNsGKs361b4w31YfwqFbF77VBaqAWjh63Xq8Mw==", "0621213322", false, "9bd3e670-ffd0-4af8-a9ea-7b21b00f8560", false, "gagisa631", 1200.0 },
                    { "642e063e-72ed-421a-86cb-b58253ea0592", 0, "b202921a-50ac-4164-a336-0d4ad61e3575", new DateTime(1992, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "losmikralj@gmail.com", false, "Milos", 0, "Kraljevic", 43.72170332787757, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (43.72170332787757 20.687793412815488)"), false, null, 20.687793412815488, "LOSMIKRALJ@GMAIL.COM", "LOSMIKRALJEVO13", "AQAAAAEAACcQAAAAEGR0DbZgDSRjWsi01PTfKCyikL0L7E/xkT38DGgq24did9s3BBX7FQbsvq04U5Dz2g==", "0625393822", false, "bb093880-b537-4b15-895f-f1ca7a9406fd", false, "losmikraljevo13", 500.0 },
                    { "8389e296-f8c6-47a7-8da2-9cbf136a1957", 0, "54179fb4-d323-4b61-8a5c-07253abd6846", new DateTime(1993, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "konstantin2@gmail.com", false, "Konstantin", 0, "Aleksic", 44.803761680454684, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.803761680454684 20.473061427202264)"), false, null, 20.473061427202264, "KONSTANTIN2@GMAIL.COM", "KONSTALE57", "AQAAAAEAACcQAAAAEGiDy8fb/nzveyONKOl9XqXIBfuBYVaj9NJMnSfufAuNiaW05r8Gfxd0AFU5OcRpig==", "0632258333", false, "6e4df672-cb15-486d-8abf-416615ef7324", false, "konstale57", 850.0 },
                    { "8a961647-b75a-4879-add0-c386861c7fd8", 0, "89ca2b64-b6a3-41a8-b286-0bfb9392ef53", new DateTime(1992, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "katarince@gmail.com", false, "Katarina", 1, "Kraljevic", 43.72498648648299, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (43.72498648648299 20.689328418664715)"), false, null, 20.689328418664715, "KATARINCE@GMAIL.COM", "KATARINAKRALJEVO2", "AQAAAAEAACcQAAAAEErwtAbi38/IoqvFsfw2JTtX56pYNqsd2wBrk9tgaqg7AJj6iLi/M3cdm0e0KCDlnA==", "0638976565", false, "e31bf380-71f1-47a5-a945-5af38740e115", false, "katarinakraljevo2", 1250.0 },
                    { "994098e5-95a7-4bda-bcde-be4be98335ca", 0, "ad14ae6c-ed8c-401d-883b-9050301fcf40", new DateTime(1975, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "djole.18@yahoo.com", false, "Djordje", 0, "Stojkovic", 44.772598104106912, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77259810410691 20.41362509637708)"), false, null, 20.413625096377078, "DJOLE.18@YAHOO.COM", "DJOLENCE18", "AQAAAAEAACcQAAAAEEl2YfZtZDA6apRQacoLZQOxsrSzoGf4RFoBMtpAkwcSGbL5/u+YxaG7el8EV6e/Bg==", "0621213325", false, "9535a591-fed1-46bf-abfe-2cbf50658368", false, "djolence18", 2900.0 },
                    { "a72bc058-0c69-47fd-80f5-79cdfd48929f", 0, "53708ff4-66bf-42d3-ba59-a4655659ca5d", new DateTime(1995, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "nikolabrdo@gmail.com", false, "Nikola", 0, "Jovic", 44.804465540081985, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.804465540081985 20.471216614306098)"), false, null, 20.471216614306098, "NIKOLABRDO@GMAIL.COM", "DZEKSON24", "AQAAAAEAACcQAAAAEI6G73SlWc0HpRT/BHbowoPwUU//q3+4PUZjUYUZNAeQDJM2dTCZlzzfLLAPMC2MTA==", "0649895563", false, "336f023f-d787-42e2-a734-d9c720ccbc66", false, "dzekson24", 500.0 },
                    { "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", 0, "f2c728e5-95e0-47fc-b302-87edc2098fc4", new DateTime(1993, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "cile1337@gmail.com", false, "Nemanja", 0, "Jankovic", 44.773732706032725, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.773732706032725 20.41394474729327)"), false, null, 20.413944747293272, "CILE1337@GMAIL.COM", "CILE1337", "AQAAAAEAACcQAAAAEIn6lZmfD7GkzztCsZPysiIXY2ZQ6GieTEWyNtECLcPf0/usFV3z6KNzQLQA7Ba+mg==", "0621213324", false, "589252ec-bacf-43c2-bb8d-94bf52067f64", false, "cile1337", 1500.0 },
                    { "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", 0, "ef5d880e-a921-4e8d-b2cd-027b563996df", new DateTime(1977, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "jovance12@yahoo.com", false, "Jovan", 0, "Kraljevic", 43.723384148276722, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (43.72338414827672 20.68819654566478)"), false, null, 20.688196545664781, "JOVANCE12@YAHOO.COM", "JOVANKRALJEVO12", "AQAAAAEAACcQAAAAEAB0UnXAOQFSBFABP0j0uxoAGOtPPfk2DrFA+VhSalF+FyJdiLZRpKsr+8i0PlpiVw==", "0644445566", false, "0e750533-d85c-4e5c-a3ec-ce42531d8f4d", false, "jovankraljevo12", 2900.0 },
                    { "f23f43b1-3253-413e-81f9-44e0252bf329", 0, "7ee54945-fa6d-4c1b-8ff9-4718b36b060a", new DateTime(1992, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "ana.banana@gmail.com", false, "Ana", 1, "Cvetkovic", 44.767584959703342, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.76758495970334 20.413305445460878)"), false, null, 20.413305445460878, "ANA.BANANA@GMAIL.COM", "ANABANANA992", "AQAAAAEAACcQAAAAEO+id+nph030FdGfB1sV0Z01nBx45UtascmRQU37Yfy9g4ojKawL1qCdta/kxesi3g==", "0621213326", false, "56d15d9f-9ddc-4e53-b405-eaaeebaf9824", false, "anabanana992", 3000.0 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalIncome", "TotalLoss", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, "bd9137f9-007c-419b-9440-63f59c378c80", new DateTime(1986, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Owner", "jecam18@gmail.com", false, "Jelena", 1, "Mirkovic", false, null, "JECAM18@GMAIL.COM", "JECAM18", "AQAAAAEAACcQAAAAEHEptRRsLlIdcopLDTuFLCpREWK8RkBEOR8kVnRLAIiAWhsVG2ZbDznpGxrAY79k2w==", "0621213329", false, "683ddab4-b168-494b-9b77-b074304df6f3", 0.0, 0.0, false, "jecam18" },
                    { "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, "0d35d7aa-1d62-45c0-9085-2a6640a28bdd", new DateTime(1997, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Owner", "pera.peric@yahoo.com", false, "Petar", 0, "Peric", false, null, "PERA.PERIC@YAHOO.COM", "PERAPERIC1", "AQAAAAEAACcQAAAAEK/6FSMUy4hP3SfNREOxrrT3s0XSlfijca9VLzUrHW98VbV+1WkXL35pmeecmzBBaw==", "0621213328", false, "45ef9fbb-7e6b-456c-9fc8-360b8e81cbb7", 0.0, 0.0, false, "peraperic1" },
                    { "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, "0ffe9ed4-7fa2-4f89-89d8-3772b2b189cc", new DateTime(2001, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Owner", "micko11@yahoo.com", false, "Milos", 0, "Maksimovic", false, null, "MICKO11@YAHOO.COM", "MICKO11", "AQAAAAEAACcQAAAAEAckDhGsN4DZmD2BwwgY/1n/Zd95Ld5YyjStC04i0M/q3/MiS4QZT/+4StWNZY7URg==", "0621213327", false, "d6ecebd0-2028-4b86-8b7b-37b7d0afcd72", 0.0, 0.0, false, "micko11" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c4d17476-bd1d-412b-b989-009ab6d3d167", 0, "ac803e7d-81fd-41f8-ad36-1be07e396872", "IdentityUser", "admin@admin.com", false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAEJY10/l4QzsYmQXumM4WFQ/CPUDuiZpYghjUMpbFZOaV7Ej5OeEMjzTkvmtPoU0a+A==", null, false, "5fe96067-d735-41fe-8d32-e7a7103a05f6", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "ee047c93-0281-4db6-8121-7b7f198b61b5" },
                    { 2, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "92a11246-aaef-495f-8dc9-51a5ede21d7f" },
                    { 3, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner", "5d38e7fc-ce18-4e36-b6d1-b3e251d58108" },
                    { 4, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "0451bf9c-489b-454e-bdd4-7b4d84bec61c" },
                    { 5, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "5de2cc4b-6449-40e2-90b6-c449b8c42ac7" },
                    { 6, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "449dc796-edaa-4afc-9ee2-1a347aba955e" },
                    { 7, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "c1806a9b-f01b-4d6b-ae76-0985be8d9bae" },
                    { 8, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "994098e5-95a7-4bda-bcde-be4be98335ca" },
                    { 9, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "f23f43b1-3253-413e-81f9-44e0252bf329" },
                    { 10, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "43be6494-af9d-4a17-9eed-5d1e03fe9c10" },
                    { 11, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "a72bc058-0c69-47fd-80f5-79cdfd48929f" },
                    { 12, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "4ff9ec50-0bf2-437a-9040-fa9c73b187f3" },
                    { 13, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "0d2c8a28-6268-4c63-b858-dc2e1118a776" },
                    { 14, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "8389e296-f8c6-47a7-8da2-9cbf136a1957" },
                    { 15, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc" },
                    { 16, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "8a961647-b75a-4879-add0-c386861c7fd8" },
                    { 17, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer", "642e063e-72ed-421a-86cb-b58253ea0592" },
                    { 18, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c4d17476-bd1d-412b-b989-009ab6d3d167" }
                });

            migrationBuilder.InsertData(
                table: "CarWashes",
                columns: new[] { "Id", "ClosingHour", "Latitude", "Location", "Longitude", "Name", "OpeningHour", "OwnerId", "Rating", "Size", "Votes" },
                values: new object[,]
                {
                    { 1, 20L, 44.771268509157252, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77126850915725 20.414552591253408)"), 20.414552591253408, "Perionica Micko", 8L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 2L, 0 },
                    { 2, 20L, 44.78255120049036, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.78255120049036 20.4101593046156)"), 20.4101593046156, "Perionica Micko2", 8L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 3L, 0 },
                    { 3, 17L, 44.805035505102715, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.805035505102715 20.470831428714185)"), 20.470831428714185, "Micko Na Vracaru", 6L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 1L, 0 },
                    { 4, 15L, 44.804394516103528, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.80439451610353 20.468022352805452)"), 20.468022352805452, "Micko Na Vracaru O5", 4L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 3L, 0 },
                    { 5, 20L, 43.725370954057006, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (43.725370954057006 20.683133458290914)"), 20.683133458290914, "Micko KV", 8L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 3L, 0 },
                    { 6, 20L, 43.721253991930219, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (43.72125399193022 20.687071480762818)"), 20.687071480762818, "Micko KV2", 8L, "ee047c93-0281-4db6-8121-7b7f198b61b5", 0, 1L, 0 },
                    { 7, 17L, 44.771268509157252, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77126850915725 20.414552591253408)"), 20.414552591253408, "Perina Periona", 5L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 2L, 0 },
                    { 8, 20L, 44.779756863173013, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77975686317301 20.41339361214433)"), 20.413393612144329, "PeraPereAutomobile", 8L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 3L, 0 },
                    { 9, 18L, 44.784000401967532, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.78400040196753 20.41585032989079)"), 20.415850329890791, "Perionica Pera", 6L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 1L, 0 },
                    { 10, 24L, 44.816561875678282, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.81656187567828 20.461417167549037)"), 20.461417167549037, "Pera NonStop", 0L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 6L, 0 },
                    { 11, 24L, 44.808697785906617, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.80869778590662 20.45480861730723)"), 20.454808617307229, "Pera NonStop2", 0L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 3L, 0 },
                    { 12, 20L, 44.847545291732203, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.8475452917322 20.395486311872737)"), 20.395486311872737, "Perina Periona 2", 8L, "92a11246-aaef-495f-8dc9-51a5ede21d7f", 0, 1L, 0 },
                    { 13, 21L, 44.768828994654449, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.76882899465445 20.414584129639266)"), 20.414584129639266, "Jeca Automoto", 7L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 2L, 0 },
                    { 14, 20L, 44.77361623450264, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.77361623450264 20.413373792352697)"), 20.413373792352697, "Jeca Automoto 2", 8L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 2L, 0 },
                    { 15, 17L, 44.781900904280057, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.78190090428006 20.41333056605157)"), 20.41333056605157, "PeriJeca", 9L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 2L, 0 },
                    { 16, 20L, 44.834416392785464, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.834416392785464 20.372848747792126)"), 20.372848747792126, "Super Jeca", 8L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 2L, 0 },
                    { 17, 24L, 44.803646640783938, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.80364664078394 20.405706618829374)"), 20.405706618829374, "Jeca Blokovi", 0L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 3L, 0 },
                    { 18, 24L, 44.815004820668065, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (44.815004820668065 20.435563120242296)"), 20.435563120242296, "Jeca Blokovi 2", 0L, "5d38e7fc-ce18-4e36-b6d1-b3e251d58108", 0, 1L, 0 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CarWashId", "CustomerId", "Date", "DateCreated", "EndHour", "IsFinished", "Rating", "ServiceType", "StartHour", "Status" },
                values: new object[,]
                {
                    { 1, 1, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 2, 1, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 3, 1, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 4, 1, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 5, 1, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 6, 2, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 7, 2, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 8, 2, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 9, 2, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 10, 2, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 11, 3, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 12, 3, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 13, 3, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 14, 3, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 15, 3, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 16, 4, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 17, 4, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 18, 4, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 4L, "Premium", 4, 1 },
                    { 19, 4, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, false, 0L, "Premium", 12, 0 },
                    { 20, 4, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 21, 5, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 22, 5, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 23, 5, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 24, 5, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 25, 5, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 26, 6, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 27, 6, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 28, 6, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 29, 6, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 30, 6, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 31, 7, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 32, 7, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 33, 7, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 34, 7, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 35, 7, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 36, 8, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 37, 8, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 38, 8, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 39, 8, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 40, 8, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 41, 9, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 42, 9, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CarWashId", "CustomerId", "Date", "DateCreated", "EndHour", "IsFinished", "Rating", "ServiceType", "StartHour", "Status" },
                values: new object[,]
                {
                    { 43, 9, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 44, 9, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 45, 9, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 46, 10, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 47, 10, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 48, 10, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 4L, "Premium", 4, 1 },
                    { 49, 10, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, false, 0L, "Premium", 12, 0 },
                    { 50, 10, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 51, 11, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 52, 11, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 53, 11, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 54, 11, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 55, 11, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 56, 12, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 57, 12, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 58, 12, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 59, 12, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 60, 12, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 61, 13, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 62, 13, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 63, 13, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 64, 13, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 65, 13, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 66, 14, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 67, 14, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 68, 14, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 69, 14, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 70, 14, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 71, 15, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 72, 15, "5de2cc4b-6449-40e2-90b6-c449b8c42ac7", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 73, 15, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 14, 1 },
                    { 74, 15, "c1806a9b-f01b-4d6b-ae76-0985be8d9bae", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 75, 15, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 76, 16, "f23f43b1-3253-413e-81f9-44e0252bf329", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 77, 16, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 78, 16, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, true, 4L, "Premium", 4, 1 },
                    { 79, 16, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, false, 0L, "Premium", 12, 0 },
                    { 80, 16, "0d2c8a28-6268-4c63-b858-dc2e1118a776", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 81, 17, "8389e296-f8c6-47a7-8da2-9cbf136a1957", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 82, 17, "e2639dbd-0c0c-4d61-9476-c7118e1d1fbc", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 83, 17, "8a961647-b75a-4879-add0-c386861c7fd8", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 84, 17, "642e063e-72ed-421a-86cb-b58253ea0592", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CarWashId", "CustomerId", "Date", "DateCreated", "EndHour", "IsFinished", "Rating", "ServiceType", "StartHour", "Status" },
                values: new object[,]
                {
                    { 85, 17, "0451bf9c-489b-454e-bdd4-7b4d84bec61c", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 },
                    { 86, 18, "449dc796-edaa-4afc-9ee2-1a347aba955e", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 0L, "Regular", 8, 2 },
                    { 87, 18, "994098e5-95a7-4bda-bcde-be4be98335ca", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, true, 5L, "Extended", 11, 1 },
                    { 88, 18, "43be6494-af9d-4a17-9eed-5d1e03fe9c10", new DateTime(2022, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, 4L, "Premium", 12, 1 },
                    { 89, 18, "a72bc058-0c69-47fd-80f5-79cdfd48929f", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, false, 0L, "Premium", 14, 0 },
                    { 90, 18, "4ff9ec50-0bf2-437a-9040-fa9c73b187f3", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 0L, "Premium", 10, 0 }
                });

            migrationBuilder.InsertData(
                table: "Revenues",
                columns: new[] { "CarWashId", "CurrentValue", "DailyIncome", "MonthlyIncome", "WeeklyIncome" },
                values: new object[,]
                {
                    { 1, 0.0, 0.0, 0.0, 0.0 },
                    { 2, 0.0, 0.0, 0.0, 0.0 },
                    { 3, 0.0, 0.0, 0.0, 0.0 },
                    { 4, 0.0, 0.0, 0.0, 0.0 },
                    { 5, 0.0, 0.0, 0.0, 0.0 },
                    { 6, 0.0, 0.0, 0.0, 0.0 },
                    { 7, 0.0, 0.0, 0.0, 0.0 },
                    { 8, 0.0, 0.0, 0.0, 0.0 },
                    { 9, 0.0, 0.0, 0.0, 0.0 },
                    { 10, 0.0, 0.0, 0.0, 0.0 },
                    { 11, 0.0, 0.0, 0.0, 0.0 },
                    { 12, 0.0, 0.0, 0.0, 0.0 },
                    { 13, 0.0, 0.0, 0.0, 0.0 },
                    { 14, 0.0, 0.0, 0.0, 0.0 },
                    { 15, 0.0, 0.0, 0.0, 0.0 },
                    { 16, 0.0, 0.0, 0.0, 0.0 },
                    { 17, 0.0, 0.0, 0.0, 0.0 },
                    { 18, 0.0, 0.0, 0.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "CarWashId", "ServiceType", "Duration", "Price" },
                values: new object[,]
                {
                    { 1, "Extended", 1, 250.0 },
                    { 1, "Premium", 2, 300.0 },
                    { 1, "Regular", 1, 100.0 },
                    { 2, "Extended", 1, 250.0 },
                    { 2, "Premium", 2, 300.0 },
                    { 2, "Regular", 1, 100.0 },
                    { 3, "Extended", 1, 250.0 },
                    { 3, "Premium", 2, 300.0 },
                    { 3, "Regular", 1, 100.0 },
                    { 4, "Extended", 1, 250.0 },
                    { 4, "Premium", 2, 300.0 },
                    { 4, "Regular", 1, 100.0 },
                    { 5, "Extended", 1, 250.0 },
                    { 5, "Premium", 2, 300.0 },
                    { 5, "Regular", 1, 100.0 },
                    { 6, "Extended", 1, 250.0 },
                    { 6, "Premium", 2, 300.0 },
                    { 6, "Regular", 1, 100.0 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "CarWashId", "ServiceType", "Duration", "Price" },
                values: new object[,]
                {
                    { 7, "Extended", 1, 250.0 },
                    { 7, "Premium", 2, 300.0 },
                    { 7, "Regular", 1, 100.0 },
                    { 8, "Extended", 1, 250.0 },
                    { 8, "Premium", 2, 300.0 },
                    { 8, "Regular", 1, 100.0 },
                    { 9, "Extended", 1, 250.0 },
                    { 9, "Premium", 2, 300.0 },
                    { 9, "Regular", 1, 100.0 },
                    { 10, "Extended", 1, 250.0 },
                    { 10, "Premium", 2, 300.0 },
                    { 10, "Regular", 1, 100.0 },
                    { 11, "Extended", 1, 250.0 },
                    { 11, "Premium", 2, 300.0 },
                    { 11, "Regular", 1, 100.0 },
                    { 12, "Extended", 1, 250.0 },
                    { 12, "Premium", 2, 300.0 },
                    { 12, "Regular", 1, 100.0 },
                    { 13, "Extended", 1, 250.0 },
                    { 13, "Premium", 2, 300.0 },
                    { 13, "Regular", 1, 100.0 },
                    { 14, "Extended", 1, 250.0 },
                    { 14, "Premium", 2, 300.0 },
                    { 14, "Regular", 1, 100.0 },
                    { 15, "Extended", 1, 250.0 },
                    { 15, "Premium", 2, 300.0 },
                    { 15, "Regular", 1, 100.0 },
                    { 16, "Extended", 1, 250.0 },
                    { 16, "Premium", 2, 300.0 },
                    { 16, "Regular", 1, 100.0 },
                    { 17, "Extended", 1, 250.0 },
                    { 17, "Premium", 2, 300.0 },
                    { 17, "Regular", 1, 100.0 },
                    { 18, "Extended", 1, 250.0 },
                    { 18, "Premium", 2, 300.0 },
                    { 18, "Regular", 1, 100.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CarWashId",
                table: "Appointments",
                column: "CarWashId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CarWashes_OwnerId",
                table: "CarWashes",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Revenues");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CarWashes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
