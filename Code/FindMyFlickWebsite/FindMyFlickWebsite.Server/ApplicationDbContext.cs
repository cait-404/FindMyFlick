using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Identity;

namespace FindMyFlickWebsite.Server
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Use scaffolded DataModels (DB-first types)
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieGenre> MovieGenres { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<MovieStreaming> MovieStreamings { get; set; } = null!;
        public DbSet<StreamingProvider> StreamingProviders { get; set; } = null!;

        // Added DbSets
        public DbSet<MovieCast> MovieCasts { get; set; } = null!;
        public DbSet<MovieCrew> MovieCrews { get; set; } = null!;
        public DbSet<MovieKeyword> MovieKeywords { get; set; } = null!;
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Keyword> Keywords { get; set; } = null!;

        // Collections
        public DbSet<Collection> Collections { get; set; } = null!;
        public DbSet<MovieCollection> MovieCollections { get; set; } = null!;

        // Add DtddOverride DbSet
        public DbSet<DtddOverride> DtddOverrides { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity mappings are configured here

            // Movie (movies)
            modelBuilder.Entity<Movie>(eb =>
            {
                eb.ToTable("movies");
                eb.HasKey(e => e.ImdbId).HasName("pk_movies_imdb_id");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbId).HasColumnName("tmdb_id");
                eb.Property(e => e.Title).HasColumnName("title");
                eb.Property(e => e.ReleaseYear).HasColumnName("release_year");
                eb.Property(e => e.MpaaRating).HasColumnName("mpaa_rating");
                eb.Property(e => e.RuntimeMinutes).HasColumnName("runtime_minutes");
                eb.Property(e => e.PlotSummary).HasColumnName("plot_summary");
                eb.Property(e => e.PosterUrl).HasColumnName("poster_url");
                eb.Property(e => e.OriginalLanguage).HasColumnName("original_language");
                eb.Property(e => e.MediaType).HasColumnName("media_type");
                eb.Property(e => e.Tagline).HasColumnName("tagline");
                eb.Property(e => e.Status).HasColumnName("status");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            });

            // MovieGenre (movie_genres) — bridge movie <-> genre
            modelBuilder.Entity<MovieGenre>(eb =>
            {
                eb.ToTable("movie_genres");
                eb.HasKey(e => new { e.ImdbId, e.TmdbGenreId }).HasName("pk_movie_genres");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbGenreId).HasColumnName("tmdb_genre_id");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieGenres)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_genres_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbGenre)
                    .WithMany(g => g.MovieGenres)
                    .HasForeignKey(e => e.TmdbGenreId)
                    .HasConstraintName("fk_movie_genres_genres_tmdb_genre_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Genre (genres)
            modelBuilder.Entity<Genre>(eb =>
            {
                eb.ToTable("genres");
                eb.HasKey(e => e.TmdbGenreId).HasName("pk_genres");

                eb.Property(e => e.TmdbGenreId).HasColumnName("tmdb_genre_id");
                eb.Property(e => e.GenreName).HasColumnName("genre_name");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            });

            // MovieStreaming (movie_streaming) — availability bridge
            modelBuilder.Entity<MovieStreaming>(eb =>
            {
                eb.ToTable("movie_streaming");
                eb.HasKey(e => new { e.ImdbId, e.TmdbProviderId, e.OfferType }).HasName("pk_movie_streaming");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbProviderId).HasColumnName("tmdb_provider_id");
                eb.Property(e => e.OfferType).HasColumnName("offer_type");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieStreamings)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_streaming_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbProvider)
                    .WithMany(p => p.MovieStreamings)
                    .HasForeignKey(e => e.TmdbProviderId)
                    .HasConstraintName("fk_movie_streaming_providers_tmdb_provider_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // StreamingProvider (streaming_providers)
            modelBuilder.Entity<StreamingProvider>(eb =>
            {
                eb.ToTable("streaming_providers");
                eb.HasKey(e => e.TmdbProviderId).HasName("pk_streaming_providers");

                eb.Property(e => e.TmdbProviderId).HasColumnName("tmdb_provider_id");
                eb.Property(e => e.ProviderName).HasColumnName("provider_name");
                eb.Property(e => e.LogoPath).HasColumnName("logo_path");
                eb.Property(e => e.DisplayPriority).HasColumnName("display_priority");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            });

            // Person (people)
            modelBuilder.Entity<Person>(eb =>
            {
                eb.ToTable("people");
                eb.HasKey(e => e.TmdbPersonId).HasName("pk_people");

                eb.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");
                eb.Property(e => e.PersonName).HasColumnName("person_name");
                eb.Property(e => e.KnownForDepartment).HasColumnName("known_for_department");
                eb.Property(e => e.ProfileUrl).HasColumnName("profile_url");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            });

            // MovieCast (movie_cast)
            modelBuilder.Entity<MovieCast>(eb =>
            {
                eb.ToTable("movie_cast");
                eb.HasKey(e => e.TmdbCreditId).HasName("pk_movie_cast");

                eb.Property(e => e.TmdbCreditId).HasColumnName("tmdb_credit_id");
                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");
                eb.Property(e => e.CharacterName).HasColumnName("character_name");
                eb.Property(e => e.CastOrder).HasColumnName("cast_order");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieCasts)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_cast_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbPerson)
                    .WithMany(p => p.MovieCasts)
                    .HasForeignKey(e => e.TmdbPersonId)
                    .HasConstraintName("fk_movie_cast_people_tmdb_person_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MovieCrew (movie_crew)
            modelBuilder.Entity<MovieCrew>(eb =>
            {
                eb.ToTable("movie_crew");
                eb.HasKey(e => e.TmdbCreditId).HasName("pk_movie_crew");

                eb.Property(e => e.TmdbCreditId).HasColumnName("tmdb_credit_id");
                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");
                eb.Property(e => e.Department).HasColumnName("department");
                eb.Property(e => e.Job).HasColumnName("job");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieCrews)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_crew_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbPerson)
                    .WithMany(p => p.MovieCrews)
                    .HasForeignKey(e => e.TmdbPersonId)
                    .HasConstraintName("fk_movie_crew_people_tmdb_person_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Keyword (keywords)
            modelBuilder.Entity<Keyword>(eb =>
            {
                eb.ToTable("keywords");
                eb.HasKey(e => e.TmdbKeywordId).HasName("pk_keywords");

                eb.Property(e => e.TmdbKeywordId).HasColumnName("tmdb_keyword_id");
                eb.Property(e => e.KeywordName).HasColumnName("keyword_name");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
            });

            // MovieKeyword (movie_keywords) — bridge movie <-> keyword
            modelBuilder.Entity<MovieKeyword>(eb =>
            {
                eb.ToTable("movie_keywords");
                eb.HasKey(e => new { e.ImdbId, e.TmdbKeywordId }).HasName("pk_movie_keywords");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbKeywordId).HasColumnName("tmdb_keyword_id");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieKeywords)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_keywords_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbKeyword)
                    .WithMany(k => k.MovieKeywords)
                    .HasForeignKey(e => e.TmdbKeywordId)
                    .HasConstraintName("fk_movie_keywords_keywords_tmdb_keyword_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Collection (collections)
            modelBuilder.Entity<Collection>(eb =>
            {
                eb.ToTable("collections");
                eb.HasKey(e => e.TmdbCollectionId).HasName("pk_collections");

                eb.Property(e => e.TmdbCollectionId).HasColumnName("tmdb_collection_id");
                eb.Property(e => e.CollectionName).HasColumnName("collection_name");
                eb.Property(e => e.PosterUrl).HasColumnName("poster_url");
                eb.Property(e => e.BackdropUrl).HasColumnName("backdrop_url");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

                eb.HasMany(e => e.MovieCollections)
                    .WithOne(mc => mc.TmdbCollection)
                    .HasForeignKey(mc => mc.TmdbCollectionId)
                    .HasConstraintName("fk_movie_collections_collections_tmdb_collection_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MovieCollection (movie_collections) — movie -> collection (one per movie)
            modelBuilder.Entity<MovieCollection>(eb =>
            {
                eb.ToTable("movie_collections");
                eb.HasKey(e => e.ImdbId).HasName("pk_movie_collections");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.TmdbCollectionId).HasColumnName("tmdb_collection_id");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithOne(m => m.MovieCollection)
                    .HasForeignKey<MovieCollection>(e => e.ImdbId)
                    .HasConstraintName("fk_movie_collections_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.TmdbCollection)
                    .WithMany(c => c.MovieCollections)
                    .HasForeignKey(e => e.TmdbCollectionId)
                    .HasConstraintName("fk_movie_collections_collections_tmdb_collection_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // DtddOverride configuration (moved to its own table to avoid colliding with movie_warnings)
            modelBuilder.Entity<DtddOverride>(eb =>
            {
                eb.ToTable("dtdd_overrides");

                eb.HasKey(e => new { e.ImdbId, e.DtddMediaId }).HasName("pk_dtdd_overrides");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.DtddMediaId).HasColumnName("dtdd_media_id");
                eb.Property(e => e.Note).HasColumnName("note");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

                // Configure as one-to-one so the foreign key lives on DtddOverride (ImdbId),
                // preventing EF from creating shadow FK columns on Movie (like DtddOverrideDtddMediaId).
                eb.HasOne(e => e.Imdb)
                    .WithOne(m => m.DtddOverride)
                    .HasForeignKey<DtddOverride>(e => e.ImdbId)
                    .HasConstraintName("fk_dtdd_overrides_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MovieDtddTitle (movie_dtdd_titles) — give it a primary key
            modelBuilder.Entity<MovieDtddTitle>(eb =>
            {
                eb.ToTable("movie_dtdd_titles");

                eb.HasKey(e => e.ImdbId).HasName("pk_movie_dtdd_titles");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.DtddTitleId).HasColumnName("dtdd_title_id");
                eb.Property(e => e.MatchMethod).HasColumnName("match_method");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.DtddMediaId).HasColumnName("dtdd_media_id");
                eb.Property(e => e.DtddImdbId).HasColumnName("dtdd_imdb_id");
                eb.Property(e => e.DtddTmdbId).HasColumnName("dtdd_tmdb_id");
                eb.Property(e => e.DtddTitle).HasColumnName("dtdd_title");
                eb.Property(e => e.DtddReleaseYear).HasColumnName("dtdd_release_year");
                eb.Property(e => e.MatchScore).HasColumnName("match_score");

                eb.HasOne(e => e.Imdb)
                    .WithOne(m => m.MovieDtddTitle)
                    .HasForeignKey<MovieDtddTitle>(e => e.ImdbId)
                    .HasConstraintName("fk_movie_dtdd_titles_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Warning (warnings) — give it a primary key and self-reference mapping
            modelBuilder.Entity<Warning>(eb =>
            {
                eb.ToTable("warnings");

                eb.HasKey(e => e.DtddTopicId).HasName("pk_warnings");

                eb.Property(e => e.DtddTopicId).HasColumnName("dtdd_topic_id");
                eb.Property(e => e.TopicName).HasColumnName("topic_name");
                eb.Property(e => e.TopicType).HasColumnName("topic_type");
                eb.Property(e => e.ParentDtddTopicId).HasColumnName("parent_dtdd_topic_id");
                eb.Property(e => e.Tier).HasColumnName("tier");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.ParentDtddTopic)
                    .WithMany(p => p.InverseParentDtddTopic)
                    .HasForeignKey(e => e.ParentDtddTopicId)
                    .HasConstraintName("fk_warnings_parent_dtdd_topic")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // MovieWarning (movie_warnings) — bridge movie <-> warning
            modelBuilder.Entity<MovieWarning>(eb =>
            {
                eb.ToTable("movie_warnings");

                eb.HasKey(e => new { e.ImdbId, e.DtddTopicId }).HasName("pk_movie_warnings");

                eb.Property(e => e.ImdbId).HasColumnName("imdb_id");
                eb.Property(e => e.DtddTopicId).HasColumnName("dtdd_topic_id");
                eb.Property(e => e.Source).HasColumnName("source");
                eb.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                eb.Property(e => e.Answer).HasColumnName("answer");
                eb.Property(e => e.IsSpoiler).HasColumnName("is_spoiler");
                eb.Property(e => e.WarningComment).HasColumnName("warning_comment");
                eb.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");

                eb.HasOne(e => e.Imdb)
                    .WithMany(m => m.MovieWarnings)
                    .HasForeignKey(e => e.ImdbId)
                    .HasConstraintName("fk_movie_warnings_movies_imdb")
                    .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(e => e.DtddTopic)
                    .WithMany(w => w.MovieWarnings)
                    .HasForeignKey(e => e.DtddTopicId)
                    .HasConstraintName("fk_movie_warnings_warnings_dtdd_topic_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Optional: configure other scaffolded types (views, warnings, etc.) as needed.
        }
    }
}