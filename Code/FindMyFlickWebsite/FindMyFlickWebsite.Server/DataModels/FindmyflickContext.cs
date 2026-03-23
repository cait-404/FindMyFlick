using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FindMyFlickWebsite.Server.DataModels;

public partial class FindmyflickContext : DbContext
{
    public FindmyflickContext()
    {
    }

    public FindmyflickContext(DbContextOptions<FindmyflickContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<DtddOverride> DtddOverrides { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieCast> MovieCasts { get; set; }

    public virtual DbSet<MovieCollection> MovieCollections { get; set; }

    public virtual DbSet<MovieCrew> MovieCrews { get; set; }

    public virtual DbSet<MovieDtddTitle> MovieDtddTitles { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<MovieKeyword> MovieKeywords { get; set; }

    public virtual DbSet<MovieStreaming> MovieStreamings { get; set; }

    public virtual DbSet<MovieWarning> MovieWarnings { get; set; }

    public virtual DbSet<MoviesWithWarning> MoviesWithWarnings { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<StreamingProvider> StreamingProviders { get; set; }

    public virtual DbSet<VMovieCollection> VMovieCollections { get; set; }

    public virtual DbSet<VMovieCollectionsAgg> VMovieCollectionsAggs { get; set; }

    public virtual DbSet<VMovieCreditsAgg> VMovieCreditsAggs { get; set; }

    public virtual DbSet<VMovieKeyword> VMovieKeywords { get; set; }

    public virtual DbSet<VMovieKeywordsAgg> VMovieKeywordsAggs { get; set; }

    public virtual DbSet<VMoviePeopleCredit> VMoviePeopleCredits { get; set; }

    public virtual DbSet<VMovieWarning> VMovieWarnings { get; set; }

    public virtual DbSet<VMovieWarningsAgg> VMovieWarningsAggs { get; set; }

    public virtual DbSet<VMoviesStreamableU> VMoviesStreamableUs { get; set; }

    public virtual DbSet<VMoviesStreamableUsAgg> VMoviesStreamableUsAggs { get; set; }

    public virtual DbSet<VPeopleSearchCard> VPeopleSearchCards { get; set; }

    public virtual DbSet<Warning> Warnings { get; set; }

    public virtual DbSet<PlotTag> PlotTags { get; set; }

    public virtual DbSet<MoviePlotTag> MoviePlotTags { get; set; }

    public virtual DbSet<MoviePlotTagVote> MoviePlotTagVotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=findmyflick;Username=postgres;Password=p@ssw0rd;SSL Mode=Prefer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.TmdbCollectionId).HasName("collections_pkey");

            entity.ToTable("collections");

            entity.HasIndex(e => e.CollectionName, "idx_collections_name");

            entity.Property(e => e.TmdbCollectionId)
                .ValueGeneratedNever()
                .HasColumnName("tmdb_collection_id");
            entity.Property(e => e.BackdropUrl).HasColumnName("backdrop_url");
            entity.Property(e => e.CollectionName).HasColumnName("collection_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<DtddOverride>(entity =>
        {
            entity.HasKey(e => e.ImdbId).HasName("dtdd_overrides_pkey");

            entity.ToTable("dtdd_overrides");

            entity.HasIndex(e => e.DtddMediaId, "dtdd_overrides_dtdd_media_id_key").IsUnique();

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DtddMediaId).HasColumnName("dtdd_media_id");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Imdb).WithOne(p => p.DtddOverride)
                .HasForeignKey<DtddOverride>(d => d.ImdbId)
                .HasConstraintName("dtdd_overrides_imdb_id_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.TmdbGenreId).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.GenreName, "genres_genre_name_key").IsUnique();

            entity.Property(e => e.TmdbGenreId)
                .ValueGeneratedNever()
                .HasColumnName("tmdb_genre_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.GenreName).HasColumnName("genre_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Keyword>(entity =>
        {
            entity.HasKey(e => e.TmdbKeywordId).HasName("keywords_pkey");

            entity.ToTable("keywords");

            entity.HasIndex(e => e.KeywordName, "idx_keywords_name");

            entity.Property(e => e.TmdbKeywordId)
                .ValueGeneratedNever()
                .HasColumnName("tmdb_keyword_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.KeywordName).HasColumnName("keyword_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.ImdbId).HasName("movies_pkey");

            entity.ToTable("movies");

            entity.HasIndex(e => e.ReleaseYear, "idx_movies_release_year");

            entity.HasIndex(e => e.Title, "idx_movies_title");

            entity.HasIndex(e => e.TmdbId, "idx_movies_tmdb_id");

            entity.HasIndex(e => e.TmdbId, "movies_tmdb_id_key").IsUnique();

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MediaType)
                .HasMaxLength(10)
                .HasDefaultValueSql("'movie'::character varying")
                .HasColumnName("media_type");
            entity.Property(e => e.MpaaRating)
                .HasMaxLength(10)
                .HasColumnName("mpaa_rating");
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("original_language");
            entity.Property(e => e.PlotSummary).HasColumnName("plot_summary");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtime_minutes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tagline).HasColumnName("tagline");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<MovieCast>(entity =>
        {
            entity.HasKey(e => e.TmdbCreditId).HasName("movie_cast_pkey");

            entity.ToTable("movie_cast");

            entity.HasIndex(e => e.ImdbId, "idx_movie_cast_imdb_id");

            entity.HasIndex(e => e.TmdbPersonId, "idx_movie_cast_person");

            entity.Property(e => e.TmdbCreditId).HasColumnName("tmdb_credit_id");
            entity.Property(e => e.CastOrder).HasColumnName("cast_order");
            entity.Property(e => e.CharacterName).HasColumnName("character_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieCasts)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_cast_movie_fk");

            entity.HasOne(d => d.TmdbPerson).WithMany(p => p.MovieCasts)
                .HasForeignKey(d => d.TmdbPersonId)
                .HasConstraintName("movie_cast_person_fk");
        });

        modelBuilder.Entity<MovieCollection>(entity =>
        {
            entity.HasKey(e => e.ImdbId).HasName("movie_collections_pkey");

            entity.ToTable("movie_collections");

            entity.HasIndex(e => e.TmdbCollectionId, "idx_movie_collections_collection");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.TmdbCollectionId).HasColumnName("tmdb_collection_id");

            entity.HasOne(d => d.Imdb).WithOne(p => p.MovieCollection)
                .HasForeignKey<MovieCollection>(d => d.ImdbId)
                .HasConstraintName("movie_collections_movie_fk");

            entity.HasOne(d => d.TmdbCollection).WithMany(p => p.MovieCollections)
                .HasForeignKey(d => d.TmdbCollectionId)
                .HasConstraintName("movie_collections_collection_fk");
        });

        modelBuilder.Entity<MovieCrew>(entity =>
        {
            entity.HasKey(e => e.TmdbCreditId).HasName("movie_crew_pkey");

            entity.ToTable("movie_crew");

            entity.HasIndex(e => e.ImdbId, "idx_movie_crew_imdb_id");

            entity.HasIndex(e => e.Job, "idx_movie_crew_job");

            entity.HasIndex(e => e.TmdbPersonId, "idx_movie_crew_person");

            entity.Property(e => e.TmdbCreditId).HasColumnName("tmdb_credit_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Department).HasColumnName("department");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieCrews)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_crew_movie_fk");

            entity.HasOne(d => d.TmdbPerson).WithMany(p => p.MovieCrews)
                .HasForeignKey(d => d.TmdbPersonId)
                .HasConstraintName("movie_crew_person_fk");
        });

        modelBuilder.Entity<MovieDtddTitle>(entity =>
        {
            entity.HasKey(e => e.ImdbId).HasName("movie_dtdd_titles_pkey");

            entity.ToTable("movie_dtdd_titles");

            entity.HasIndex(e => e.DtddTmdbId, "idx_movie_dtdd_titles_dtdd_tmdb_id");

            entity.HasIndex(e => e.MatchMethod, "idx_movie_dtdd_titles_match_method");

            entity.HasIndex(e => e.DtddMediaId, "movie_dtdd_titles_dtdd_media_id_key").IsUnique();

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DtddImdbId).HasColumnName("dtdd_imdb_id");
            entity.Property(e => e.DtddMediaId).HasColumnName("dtdd_media_id");
            entity.Property(e => e.DtddReleaseYear).HasColumnName("dtdd_release_year");
            entity.Property(e => e.DtddTitle).HasColumnName("dtdd_title");
            entity.Property(e => e.DtddTitleId).HasColumnName("dtdd_title_id");
            entity.Property(e => e.DtddTmdbId).HasColumnName("dtdd_tmdb_id");
            entity.Property(e => e.MatchMethod).HasColumnName("match_method");
            entity.Property(e => e.MatchScore)
                .HasPrecision(6, 3)
                .HasColumnName("match_score");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Imdb).WithOne(p => p.MovieDtddTitle)
                .HasForeignKey<MovieDtddTitle>(d => d.ImdbId)
                .HasConstraintName("movie_dtdd_titles_movie_fk");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.HasKey(e => new { e.ImdbId, e.TmdbGenreId }).HasName("movie_genres_pkey");

            entity.ToTable("movie_genres");

            entity.HasIndex(e => e.TmdbGenreId, "idx_movie_genres_genre_id");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.TmdbGenreId).HasColumnName("tmdb_genre_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_genres_movie_fk");

            entity.HasOne(d => d.TmdbGenre).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.TmdbGenreId)
                .HasConstraintName("movie_genres_genre_fk");
        });

        modelBuilder.Entity<MovieKeyword>(entity =>
        {
            entity.HasKey(e => new { e.ImdbId, e.TmdbKeywordId }).HasName("movie_keywords_pkey");

            entity.ToTable("movie_keywords");

            entity.HasIndex(e => e.TmdbKeywordId, "idx_movie_keywords_keyword");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.TmdbKeywordId).HasColumnName("tmdb_keyword_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieKeywords)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_keywords_movie_fk");

            entity.HasOne(d => d.TmdbKeyword).WithMany(p => p.MovieKeywords)
                .HasForeignKey(d => d.TmdbKeywordId)
                .HasConstraintName("movie_keywords_keyword_fk");
        });

        modelBuilder.Entity<MovieStreaming>(entity =>
        {
            entity.HasKey(e => new { e.ImdbId, e.TmdbProviderId, e.OfferType }).HasName("movie_streaming_pkey");

            entity.ToTable("movie_streaming");

            entity.HasIndex(e => e.OfferType, "idx_movie_streaming_offer_type");

            entity.HasIndex(e => e.TmdbProviderId, "idx_movie_streaming_provider");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.TmdbProviderId).HasColumnName("tmdb_provider_id");
            entity.Property(e => e.OfferType).HasColumnName("offer_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieStreamings)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_streaming_movie_fk");

            entity.HasOne(d => d.TmdbProvider).WithMany(p => p.MovieStreamings)
                .HasForeignKey(d => d.TmdbProviderId)
                .HasConstraintName("movie_streaming_provider_fk");
        });

        modelBuilder.Entity<MovieWarning>(entity =>
        {
            entity.HasKey(e => new { e.ImdbId, e.DtddTopicId }).HasName("movie_warnings_pkey");

            entity.ToTable("movie_warnings");

            entity.HasIndex(e => e.ImdbId, "idx_movie_warnings_imdb");

            entity.HasIndex(e => e.DtddTopicId, "idx_movie_warnings_topic");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.DtddTopicId).HasColumnName("dtdd_topic_id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsSpoiler).HasColumnName("is_spoiler");
            entity.Property(e => e.Source)
                .HasDefaultValueSql("'DTDD'::text")
                .HasColumnName("source");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.WarningComment).HasColumnName("warning_comment");

            entity.HasOne(d => d.DtddTopic).WithMany(p => p.MovieWarnings)
                .HasForeignKey(d => d.DtddTopicId)
                .HasConstraintName("movie_warnings_topic_fk");

            entity.HasOne(d => d.Imdb).WithMany(p => p.MovieWarnings)
                .HasForeignKey(d => d.ImdbId)
                .HasConstraintName("movie_warnings_movie_fk");
        });

        modelBuilder.Entity<MoviesWithWarning>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("movies_with_warnings");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.MediaType)
                .HasMaxLength(10)
                .HasColumnName("media_type");
            entity.Property(e => e.MpaaRating)
                .HasMaxLength(10)
                .HasColumnName("mpaa_rating");
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("original_language");
            entity.Property(e => e.PlotSummary).HasColumnName("plot_summary");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtime_minutes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tagline).HasColumnName("tagline");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.TmdbPersonId).HasName("people_pkey");

            entity.ToTable("people");

            entity.HasIndex(e => e.PersonName, "idx_people_name");

            entity.HasIndex(e => e.PersonName, "idx_people_person_name_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.Property(e => e.TmdbPersonId)
                .ValueGeneratedNever()
                .HasColumnName("tmdb_person_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.KnownForDepartment).HasColumnName("known_for_department");
            entity.Property(e => e.PersonName).HasColumnName("person_name");
            entity.Property(e => e.ProfileUrl).HasColumnName("profile_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StreamingProvider>(entity =>
        {
            entity.HasKey(e => e.TmdbProviderId).HasName("streaming_providers_pkey");

            entity.ToTable("streaming_providers");

            entity.HasIndex(e => e.ProviderName, "idx_streaming_providers_name");

            entity.Property(e => e.TmdbProviderId)
                .ValueGeneratedNever()
                .HasColumnName("tmdb_provider_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayPriority).HasColumnName("display_priority");
            entity.Property(e => e.LogoPath).HasColumnName("logo_path");
            entity.Property(e => e.ProviderName).HasColumnName("provider_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<VMovieCollection>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_collections");

            entity.Property(e => e.CollectionBackdropUrl).HasColumnName("collection_backdrop_url");
            entity.Property(e => e.CollectionName).HasColumnName("collection_name");
            entity.Property(e => e.CollectionPosterUrl).HasColumnName("collection_poster_url");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbCollectionId).HasColumnName("tmdb_collection_id");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
        });

        modelBuilder.Entity<VMovieCollectionsAgg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_collections_agg");

            entity.Property(e => e.CollectionBackdropUrl).HasColumnName("collection_backdrop_url");
            entity.Property(e => e.CollectionName).HasColumnName("collection_name");
            entity.Property(e => e.CollectionPosterUrl).HasColumnName("collection_poster_url");
            entity.Property(e => e.ImdbIds).HasColumnName("imdb_ids");
            entity.Property(e => e.MovieCount).HasColumnName("movie_count");
            entity.Property(e => e.Titles).HasColumnName("titles");
            entity.Property(e => e.TmdbCollectionId).HasColumnName("tmdb_collection_id");
        });

        modelBuilder.Entity<VMovieCreditsAgg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_credits_agg");

            entity.Property(e => e.Directors).HasColumnName("directors");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.TopBilledCast).HasColumnName("top_billed_cast");
            entity.Property(e => e.Writers).HasColumnName("writers");
        });

        modelBuilder.Entity<VMovieKeyword>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_keywords");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.KeywordName).HasColumnName("keyword_name");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.TmdbKeywordId).HasColumnName("tmdb_keyword_id");
        });

        modelBuilder.Entity<VMovieKeywordsAgg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_keywords_agg");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.Keywords).HasColumnName("keywords");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
        });

        modelBuilder.Entity<VMoviePeopleCredit>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_people_credits");

            entity.Property(e => e.CastOrder).HasColumnName("cast_order");
            entity.Property(e => e.CharacterName).HasColumnName("character_name");
            entity.Property(e => e.CreditType).HasColumnName("credit_type");
            entity.Property(e => e.Department).HasColumnName("department");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.PersonName).HasColumnName("person_name");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbCreditId).HasColumnName("tmdb_credit_id");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");
        });

        modelBuilder.Entity<VMovieWarning>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_warnings");

            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.DtddTopicId).HasColumnName("dtdd_topic_id");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.IsSpoiler).HasColumnName("is_spoiler");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.TopicName).HasColumnName("topic_name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.WarningComment).HasColumnName("warning_comment");
        });

        modelBuilder.Entity<VMovieWarningsAgg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movie_warnings_agg");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.NoTopicIds).HasColumnName("no_topic_ids");
            entity.Property(e => e.NoTopics).HasColumnName("no_topics");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
            entity.Property(e => e.UnknownTopicIds).HasColumnName("unknown_topic_ids");
            entity.Property(e => e.UnknownTopics).HasColumnName("unknown_topics");
            entity.Property(e => e.YesTopicIds).HasColumnName("yes_topic_ids");
            entity.Property(e => e.YesTopics).HasColumnName("yes_topics");
        });

        modelBuilder.Entity<VMoviesStreamableU>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movies_streamable_us");

            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.OfferRank).HasColumnName("offer_rank");
            entity.Property(e => e.OfferType).HasColumnName("offer_type");
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("original_language");
            entity.Property(e => e.PlotSummary).HasColumnName("plot_summary");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.ProviderName).HasColumnName("provider_name");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtime_minutes");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
        });

        modelBuilder.Entity<VMoviesStreamableUsAgg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_movies_streamable_us_agg");

            entity.Property(e => e.BestOfferRank).HasColumnName("best_offer_rank");
            entity.Property(e => e.BestOfferType).HasColumnName("best_offer_type");
            entity.Property(e => e.FreeProviders).HasColumnName("free_providers");
            entity.Property(e => e.FreeWithAdsProviders).HasColumnName("free_with_ads_providers");
            entity.Property(e => e.ImdbId)
                .HasMaxLength(16)
                .HasColumnName("imdb_id");
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("original_language");
            entity.Property(e => e.PlotSummary).HasColumnName("plot_summary");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtime_minutes");
            entity.Property(e => e.SubscriptionProviders).HasColumnName("subscription_providers");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TmdbId).HasColumnName("tmdb_id");
        });

        modelBuilder.Entity<VPeopleSearchCard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_people_search_cards");

            entity.Property(e => e.ActorCredits).HasColumnName("actor_credits");
            entity.Property(e => e.DirectorCredits).HasColumnName("director_credits");
            entity.Property(e => e.IsActor).HasColumnName("is_actor");
            entity.Property(e => e.IsDirector).HasColumnName("is_director");
            entity.Property(e => e.IsProducer).HasColumnName("is_producer");
            entity.Property(e => e.IsWriter).HasColumnName("is_writer");
            entity.Property(e => e.KnownForDepartment).HasColumnName("known_for_department");
            entity.Property(e => e.PersonName).HasColumnName("person_name");
            entity.Property(e => e.ProducerCredits).HasColumnName("producer_credits");
            entity.Property(e => e.ProfileUrl).HasColumnName("profile_url");
            entity.Property(e => e.RoleLabels).HasColumnName("role_labels");
            entity.Property(e => e.TmdbPersonId).HasColumnName("tmdb_person_id");
            entity.Property(e => e.WriterCredits).HasColumnName("writer_credits");
        });

        modelBuilder.Entity<Warning>(entity =>
        {
            entity.HasKey(e => e.DtddTopicId).HasName("warnings_pkey");

            entity.ToTable("warnings");

            entity.HasIndex(e => e.ParentDtddTopicId, "idx_warnings_parent");

            entity.HasIndex(e => e.TopicName, "idx_warnings_topic_name");

            entity.Property(e => e.DtddTopicId)
                .ValueGeneratedNever()
                .HasColumnName("dtdd_topic_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ParentDtddTopicId).HasColumnName("parent_dtdd_topic_id");
            entity.Property(e => e.Tier).HasColumnName("tier");
            entity.Property(e => e.TopicName).HasColumnName("topic_name");
            entity.Property(e => e.TopicType).HasColumnName("topic_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ParentDtddTopic).WithMany(p => p.InverseParentDtddTopic)
                .HasForeignKey(d => d.ParentDtddTopicId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("warnings_parent_fk");
        });

        modelBuilder.Entity<PlotTag>(entity =>
        {
            entity.ToTable("plot_tags", schema: "public");

            entity.HasKey(e => e.PlotTagId).HasName("pk_plot_tags");

            entity.Property(e => e.PlotTagId).HasColumnName("plot_tag_id");
            entity.Property(e => e.TagText).HasColumnName("tag_text").IsRequired();
            entity.Property(e => e.TagTextNorm).HasColumnName("tag_text_norm").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            
            // unique index on normalized text
            entity.HasIndex(e => e.TagTextNorm).HasDatabaseName("idx_plot_tags_norm").IsUnique(false);
        });

        // Configure MoviePlotTag
        modelBuilder.Entity<MoviePlotTag>(entity =>
        {
            entity.ToTable("movie_plot_tags", schema: "public");

            entity.HasKey(e => new { e.ImdbId, e.PlotTagId }).HasName("pk_movie_plot_tags");

            entity.Property(e => e.ImdbId).HasColumnName("imdb_id").HasMaxLength(16);
            entity.Property(e => e.PlotTagId).HasColumnName("plot_tag_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            
            // created_by_user_id stored as text in your DB (adjust the column type if your DB differs)
            entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue("approved");

            entity.HasIndex(e => e.PlotTagId).HasDatabaseName("idx_movie_plot_tags_tag");
            entity.HasIndex(e => e.Status).HasDatabaseName("idx_movie_plot_tags_status");

            // FKs
            entity.HasOne(d => d.PlotTag)
                  .WithMany(p => p.MoviePlotTags)
                  .HasForeignKey(d => d.PlotTagId)
                  .HasConstraintName("fk_movie_plot_tags_plot_tag")
                  .OnDelete(DeleteBehavior.Cascade);

            // Movie FK (imdb_id) -> Movies table (if present in model)
            entity.HasOne(d => d.Movie)
                  .WithMany() // Movie entity already defines collections; avoid double mapping disagreements
                  .HasForeignKey(d => d.ImdbId)
                  .HasConstraintName("fk_movie_plot_tags_movie")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure MoviePlotTagVote
        modelBuilder.Entity<MoviePlotTagVote>(entity =>
        {
            entity.ToTable("movie_plot_tag_votes", schema: "public");

            entity.HasKey(e => new { e.ImdbId, e.PlotTagId, e.UserId })
                  .HasName("pk_movie_plot_tag_votes");

            entity.Property(e => e.ImdbId).HasColumnName("imdb_id").HasMaxLength(16);
            entity.Property(e => e.PlotTagId).HasColumnName("plot_tag_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Vote).HasColumnName("vote");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

            // FK -> movie_plot_tags (imdb_id, plot_tag_id)
            entity.HasOne(d => d.MoviePlotTag)
                  .WithMany(p => p.MoviePlotTagVotes)
                  .HasForeignKey(d => new { d.ImdbId, d.PlotTagId })
                  .HasConstraintName("fk_movie_plot_tag_votes_movie_plot_tags")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Allow the rest of the existing partials to run
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
