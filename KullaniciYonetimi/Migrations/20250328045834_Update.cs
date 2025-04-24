using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KullaniciYonetimi.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
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
                name: "IslemTipiListesi",
                columns: table => new
                {
                    IslemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IslemTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemTipiListesi", x => x.IslemID);
                });

            migrationBuilder.CreateTable(
                name: "KategoriListesi",
                columns: table => new
                {
                    KategoriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategoriListesi", x => x.KategoriID);
                });

            migrationBuilder.CreateTable(
                name: "SepetListesi",
                columns: table => new
                {
                    SepetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SepetTutari = table.Column<int>(type: "int", nullable: false),
                    SepetTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepetListesi", x => x.SepetID);
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
                name: "UrunListesi",
                columns: table => new
                {
                    UrunID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrunBarkod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UrunKategoriID = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunListesi", x => x.UrunID);
                    table.ForeignKey(
                        name: "FK_UrunListesi_KategoriListesi_UrunKategoriID",
                        column: x => x.UrunKategoriID,
                        principalTable: "KategoriListesi",
                        principalColumn: "KategoriID");
                });

            migrationBuilder.CreateTable(
                name: "SiparisListesi",
                columns: table => new
                {
                    SiparisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SepetID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SiparisFiyatiTutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisListesi", x => x.SiparisID);
                    table.ForeignKey(
                        name: "FK_SiparisListesi_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisListesi_SepetListesi_SepetID",
                        column: x => x.SepetID,
                        principalTable: "SepetListesi",
                        principalColumn: "SepetID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IslemlerListesi",
                columns: table => new
                {
                    IslemlerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IslemTipiID = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemlerListesi", x => x.IslemlerID);
                    table.ForeignKey(
                        name: "FK_IslemlerListesi_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IslemlerListesi_IslemTipiListesi_IslemTipiID",
                        column: x => x.IslemTipiID,
                        principalTable: "IslemTipiListesi",
                        principalColumn: "IslemID");
                    table.ForeignKey(
                        name: "FK_IslemlerListesi_UrunListesi_UrunID",
                        column: x => x.UrunID,
                        principalTable: "UrunListesi",
                        principalColumn: "UrunID");
                });

            migrationBuilder.CreateTable(
                name: "UrunFiyatListesi",
                columns: table => new
                {
                    UrunFiyatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunBarkod = table.Column<int>(type: "int", nullable: false),
                    UrunFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UrunFiyatTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunFiyatListesi", x => x.UrunFiyatID);
                    table.ForeignKey(
                        name: "FK_UrunFiyatListesi_UrunListesi_UrunBarkod",
                        column: x => x.UrunBarkod,
                        principalTable: "UrunListesi",
                        principalColumn: "UrunID");
                });

            migrationBuilder.CreateTable(
                name: "SiparisOdemeDurumListesi",
                columns: table => new
                {
                    SiparisOdemeDurumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SiparisOdemeDurumu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiparisOdemeDurumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisOdemeDurumListesi", x => x.SiparisOdemeDurumID);
                    table.ForeignKey(
                        name: "FK_SiparisOdemeDurumListesi_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiparisOdemeDurumListesi_SiparisListesi_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "SiparisListesi",
                        principalColumn: "SiparisID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiparisTeslimatDurumListesi",
                columns: table => new
                {
                    SiparisTeslimatDurumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SiparisTeslimatDurumu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiparisTeslimatDurumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisTeslimatDurumListesi", x => x.SiparisTeslimatDurumID);
                    table.ForeignKey(
                        name: "FK_SiparisTeslimatDurumListesi_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiparisTeslimatDurumListesi_SiparisListesi_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "SiparisListesi",
                        principalColumn: "SiparisID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SepetIslemleri",
                columns: table => new
                {
                    SepetIslemleriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SepetID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UrunBarkod = table.Column<int>(type: "int", nullable: false),
                    UrunFiyatID = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepetIslemleri", x => x.SepetIslemleriID);
                    table.ForeignKey(
                        name: "FK_SepetIslemleri_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SepetIslemleri_SepetListesi_SepetID",
                        column: x => x.SepetID,
                        principalTable: "SepetListesi",
                        principalColumn: "SepetID");
                    table.ForeignKey(
                        name: "FK_SepetIslemleri_UrunFiyatListesi_UrunFiyatID",
                        column: x => x.UrunFiyatID,
                        principalTable: "UrunFiyatListesi",
                        principalColumn: "UrunFiyatID");
                    table.ForeignKey(
                        name: "FK_SepetIslemleri_UrunListesi_UrunBarkod",
                        column: x => x.UrunBarkod,
                        principalTable: "UrunListesi",
                        principalColumn: "UrunID");
                });

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
                name: "IX_IslemlerListesi_IslemTipiID",
                table: "IslemlerListesi",
                column: "IslemTipiID");

            migrationBuilder.CreateIndex(
                name: "IX_IslemlerListesi_UrunID",
                table: "IslemlerListesi",
                column: "UrunID");

            migrationBuilder.CreateIndex(
                name: "IX_IslemlerListesi_UserID",
                table: "IslemlerListesi",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SepetIslemleri_SepetID",
                table: "SepetIslemleri",
                column: "SepetID");

            migrationBuilder.CreateIndex(
                name: "IX_SepetIslemleri_UrunBarkod",
                table: "SepetIslemleri",
                column: "UrunBarkod");

            migrationBuilder.CreateIndex(
                name: "IX_SepetIslemleri_UrunFiyatID",
                table: "SepetIslemleri",
                column: "UrunFiyatID");

            migrationBuilder.CreateIndex(
                name: "IX_SepetIslemleri_UserID",
                table: "SepetIslemleri",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisListesi_SepetID",
                table: "SiparisListesi",
                column: "SepetID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisListesi_UserID",
                table: "SiparisListesi",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisOdemeDurumListesi_SiparisID",
                table: "SiparisOdemeDurumListesi",
                column: "SiparisID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisOdemeDurumListesi_UserID",
                table: "SiparisOdemeDurumListesi",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisTeslimatDurumListesi_SiparisID",
                table: "SiparisTeslimatDurumListesi",
                column: "SiparisID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisTeslimatDurumListesi_UserID",
                table: "SiparisTeslimatDurumListesi",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UrunFiyatListesi_UrunBarkod",
                table: "UrunFiyatListesi",
                column: "UrunBarkod");

            migrationBuilder.CreateIndex(
                name: "IX_UrunListesi_UrunKategoriID",
                table: "UrunListesi",
                column: "UrunKategoriID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "IslemlerListesi");

            migrationBuilder.DropTable(
                name: "SepetIslemleri");

            migrationBuilder.DropTable(
                name: "SiparisOdemeDurumListesi");

            migrationBuilder.DropTable(
                name: "SiparisTeslimatDurumListesi");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "IslemTipiListesi");

            migrationBuilder.DropTable(
                name: "UrunFiyatListesi");

            migrationBuilder.DropTable(
                name: "SiparisListesi");

            migrationBuilder.DropTable(
                name: "UrunListesi");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SepetListesi");

            migrationBuilder.DropTable(
                name: "KategoriListesi");
        }
    }
}
