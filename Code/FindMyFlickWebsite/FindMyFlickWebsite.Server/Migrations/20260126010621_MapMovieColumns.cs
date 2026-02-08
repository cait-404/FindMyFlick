using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindMyFlickWebsite.Server.Migrations
{
    /// <inheritdoc />
    public partial class MapMovieColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Movies_MoviesID",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "PersonTags");

            migrationBuilder.DropTable(
                name: "PlotTags");

            migrationBuilder.DropTable(
                name: "TagVotes");

            migrationBuilder.DropTable(
                name: "TriggerTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "StreamingServices",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserRatings",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserWatchStatus",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "movies");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "movies",
                newName: "release_year");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "movies",
                newName: "plot_summary");

            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "movies",
                newName: "poster_url");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "movies",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "AgeRating",
                table: "movies",
                newName: "mpaa_rating");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "movies",
                newName: "imdb_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "movies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "media_type",
                table: "movies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "original_language",
                table: "movies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "runtime_minutes",
                table: "movies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "movies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tagline",
                table: "movies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tmdb_id",
                table: "movies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "movies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "MoviesID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "imdb_id");

            migrationBuilder.CreateTable(
                name: "movie_genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    tmdb_genre_id = table.Column<int>(type: "integer", nullable: false),
                    imdb_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_genres", x => x.id);
                    table.ForeignKey(
                        name: "FK_movie_genres_movies_imdb_id",
                        column: x => x.imdb_id,
                        principalTable: "movies",
                        principalColumn: "imdb_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tags_id = table.Column<int>(type: "integer", nullable: false),
                    tag_type = table.Column<string>(type: "text", nullable: true),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    tag_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_tags_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plot_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tags_id = table.Column<int>(type: "integer", nullable: false),
                    tag_type = table.Column<string>(type: "text", nullable: true),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    tag_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plot_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_plot_tags_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streaming_providers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    provider_name = table.Column<string>(type: "text", nullable: true),
                    imdb_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streaming_providers", x => x.id);
                    table.ForeignKey(
                        name: "FK_streaming_providers_movies_imdb_id",
                        column: x => x.imdb_id,
                        principalTable: "movies",
                        principalColumn: "imdb_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_votes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    upvotes = table.Column<int>(type: "integer", nullable: false),
                    downvotes = table.Column<int>(type: "integer", nullable: false),
                    movie_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_votes", x => x.id);
                    table.ForeignKey(
                        name: "FK_tag_votes_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "imdb_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trigger_tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tags_id = table.Column<int>(type: "integer", nullable: false),
                    tag_type = table.Column<string>(type: "text", nullable: true),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    tag_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trigger_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_trigger_tags_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movie_genres_imdb_id",
                table: "movie_genres",
                column: "imdb_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_tags_tags_id",
                table: "person_tags",
                column: "tags_id");

            migrationBuilder.CreateIndex(
                name: "IX_plot_tags_tags_id",
                table: "plot_tags",
                column: "tags_id");

            migrationBuilder.CreateIndex(
                name: "IX_streaming_providers_imdb_id",
                table: "streaming_providers",
                column: "imdb_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_votes_movie_id",
                table: "tag_votes",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_trigger_tags_tags_id",
                table: "trigger_tags",
                column: "tags_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags",
                column: "MoviesID",
                principalTable: "movies",
                principalColumn: "imdb_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags");

            migrationBuilder.DropTable(
                name: "movie_genres");

            migrationBuilder.DropTable(
                name: "person_tags");

            migrationBuilder.DropTable(
                name: "plot_tags");

            migrationBuilder.DropTable(
                name: "streaming_providers");

            migrationBuilder.DropTable(
                name: "tag_votes");

            migrationBuilder.DropTable(
                name: "trigger_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "original_language",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "runtime_minutes",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "status",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "tagline",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "tmdb_id",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "movies");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "Movies");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Movies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "release_year",
                table: "Movies",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "poster_url",
                table: "Movies",
                newName: "Poster");

            migrationBuilder.RenameColumn(
                name: "plot_summary",
                table: "Movies",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "mpaa_rating",
                table: "Movies",
                newName: "AgeRating");

            migrationBuilder.RenameColumn(
                name: "imdb_id",
                table: "Movies",
                newName: "ID");

            migrationBuilder.AddColumn<List<string>>(
                name: "Genre",
                table: "Movies",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "StreamingServices",
                table: "Movies",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "UserRatings",
                table: "Movies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "UserWatchStatus",
                table: "Movies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "MoviesID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "PersonTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagID = table.Column<int>(type: "integer", nullable: false),
                    TagName = table.Column<string>(type: "text", nullable: true),
                    TagType = table.Column<string>(type: "text", nullable: true),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonTags_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagID = table.Column<int>(type: "integer", nullable: false),
                    TagName = table.Column<string>(type: "text", nullable: true),
                    TagType = table.Column<string>(type: "text", nullable: true),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlotTags_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Downvotes = table.Column<int>(type: "integer", nullable: false),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    TagID = table.Column<int>(type: "integer", nullable: false),
                    Upvotes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagVotes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagID = table.Column<int>(type: "integer", nullable: false),
                    TagName = table.Column<string>(type: "text", nullable: true),
                    TagType = table.Column<string>(type: "text", nullable: true),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriggerTags_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "MoviesID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonTags_TagsId",
                table: "PersonTags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotTags_TagsId",
                table: "PlotTags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_TagVotes_MovieId",
                table: "TagVotes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_TriggerTags_TagsId",
                table: "TriggerTags",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Movies_MoviesID",
                table: "Tags",
                column: "MoviesID",
                principalTable: "Movies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
