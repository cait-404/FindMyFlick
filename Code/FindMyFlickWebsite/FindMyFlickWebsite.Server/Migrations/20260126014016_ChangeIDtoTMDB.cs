using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindMyFlickWebsite.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIDtoTMDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_movies_imdb_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_streaming_providers_movies_imdb_id",
                table: "streaming_providers");

            migrationBuilder.DropForeignKey(
                name: "FK_tag_votes_movies_movie_id",
                table: "tag_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            // Drop identity on imdb_id if present (idempotent)
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (
    SELECT 1
    FROM information_schema.columns
    WHERE table_schema = 'public' AND table_name = 'movies' AND column_name = 'imdb_id'
      AND is_identity = 'YES'
  ) THEN
    EXECUTE 'ALTER TABLE public.movies ALTER COLUMN imdb_id DROP IDENTITY';
  END IF;
END;
$$;
");

            // Ensure tmdb_id has a sequence/default so it can become the PK/identity target
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_class WHERE relkind = 'S' AND relname = 'movies_tmdb_id_seq') THEN
    CREATE SEQUENCE public.movies_tmdb_id_seq;
  END IF;

  PERFORM setval('public.movies_tmdb_id_seq', COALESCE((SELECT MAX(tmdb_id) FROM public.movies), 1), true);

  IF (SELECT column_default FROM information_schema.columns
      WHERE table_schema = 'public' AND table_name = 'movies' AND column_name = 'tmdb_id') IS NULL THEN
    EXECUTE 'ALTER TABLE public.movies ALTER COLUMN tmdb_id SET DEFAULT nextval(''public.movies_tmdb_id_seq'')';
  END IF;
END;
$$;
");

            migrationBuilder.AlterColumn<int>(
                name: "tmdb_id",
                table: "movies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "imdb_id",
                table: "movies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "tmdb_id");

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_movies_imdb_id",
                table: "movie_genres",
                column: "imdb_id",
                principalTable: "movies",
                principalColumn: "tmdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streaming_providers_movies_imdb_id",
                table: "streaming_providers",
                column: "imdb_id",
                principalTable: "movies",
                principalColumn: "tmdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tag_votes_movies_movie_id",
                table: "tag_votes",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "tmdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags",
                column: "MoviesID",
                principalTable: "movies",
                principalColumn: "tmdb_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movie_genres_movies_imdb_id",
                table: "movie_genres");

            migrationBuilder.DropForeignKey(
                name: "FK_streaming_providers_movies_imdb_id",
                table: "streaming_providers");

            migrationBuilder.DropForeignKey(
                name: "FK_tag_votes_movies_movie_id",
                table: "tag_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            // Remove default/sequence on tmdb_id if it was created by Up (idempotent)
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (
    SELECT 1 FROM pg_class WHERE relkind = 'S' AND relname = 'movies_tmdb_id_seq'
  ) THEN
    -- drop default if set
    IF EXISTS (
      SELECT 1 FROM information_schema.columns
      WHERE table_schema = 'public' AND table_name = 'movies' AND column_name = 'tmdb_id' AND column_default IS NOT NULL
    ) THEN
      EXECUTE 'ALTER TABLE public.movies ALTER COLUMN tmdb_id DROP DEFAULT';
    END IF;
    -- drop sequence (only if no other dependency); safe to check and drop
    IF NOT EXISTS (
      SELECT 1 FROM pg_depend d
      JOIN pg_class c ON d.objid = c.oid
      WHERE c.relkind = 'S' AND c.relname = 'movies_tmdb_id_seq'
        AND d.deptype = 'a'
    ) THEN
      EXECUTE 'DROP SEQUENCE IF EXISTS public.movies_tmdb_id_seq';
    ELSE
      -- If sequence has dependencies, don't drop to avoid breaking things.
      NULL;
    END IF;
  END IF;
END;
$$;
");

            // Re-add identity to imdb_id if not present (idempotent)
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF NOT EXISTS (
    SELECT 1 FROM information_schema.columns
    WHERE table_schema = 'public' AND table_name = 'movies' AND column_name = 'imdb_id' AND is_identity = 'YES'
  ) THEN
    BEGIN
      -- If imdb_id already has a default sequence, drop it first
      IF EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = 'public' AND table_name = 'movies' AND column_name = 'imdb_id' AND column_default IS NOT NULL
      ) THEN
        EXECUTE 'ALTER TABLE public.movies ALTER COLUMN imdb_id DROP DEFAULT';
      END IF;
      EXECUTE 'ALTER TABLE public.movies ALTER COLUMN imdb_id ADD GENERATED BY DEFAULT AS IDENTITY';
    END;
  END IF;
END;
$$;
");

            migrationBuilder.AlterColumn<int>(
                name: "imdb_id",
                table: "movies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "tmdb_id",
                table: "movies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "imdb_id");

            migrationBuilder.AddForeignKey(
                name: "FK_movie_genres_movies_imdb_id",
                table: "movie_genres",
                column: "imdb_id",
                principalTable: "movies",
                principalColumn: "imdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streaming_providers_movies_imdb_id",
                table: "streaming_providers",
                column: "imdb_id",
                principalTable: "movies",
                principalColumn: "imdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tag_votes_movies_movie_id",
                table: "tag_votes",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "imdb_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tags_movies_MoviesID",
                table: "tags",
                column: "MoviesID",
                principalTable: "movies",
                principalColumn: "imdb_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
