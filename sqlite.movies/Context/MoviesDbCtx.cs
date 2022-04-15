using Microsoft.EntityFrameworkCore;
using sqlite.movies.Models;
using System.Diagnostics;

namespace sqlite.movies.Context
{
    public partial class MoviesDbCtx : DbContext
    {
        public MoviesDbCtx()
        {
        }

        public MoviesDbCtx(DbContextOptions<MoviesDbCtx> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Keyword> Keywords { get; set; } = null!;
        public virtual DbSet<Language> Languages { get; set; } = null!;
        public virtual DbSet<LanguageRole> LanguageRoles { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MovieCast> MovieCasts { get; set; } = null!;
        public virtual DbSet<MovieCompany> MovieCompanies { get; set; } = null!;
        public virtual DbSet<MovieCrew> MovieCrews { get; set; } = null!;
        public virtual DbSet<MovieGenre> MovieGenres { get; set; } = null!;
        public virtual DbSet<MovieKeyword> MovieKeywords { get; set; } = null!;
        public virtual DbSet<MovieLanguage> MovieLanguages { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<ProductionCompany> ProductionCompanies { get; set; } = null!;
        public virtual DbSet<ProductionCountry> ProductionCountries { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=E:\\Code\\graphQlMovies\\sqlite.movies\\movies.db");
                optionsBuilder.LogTo((str) => Debug.WriteLine(str));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int")
                    .ValueGeneratedNever()
                    .HasColumnName("country_id");

                entity.Property(e => e.CountryIsoCode)
                    .HasColumnType("text")
                    .HasColumnName("country_iso_code");

                entity.Property(e => e.CountryName)
                    .HasColumnType("text")
                    .HasColumnName("country_name");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("int")
                    .ValueGeneratedNever()
                    .HasColumnName("department_id");

                entity.Property(e => e.DepartmentName).HasColumnName("department_name");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.Property(e => e.GenderId)
                    .HasColumnType("int")
                    .ValueGeneratedNever()
                    .HasColumnName("gender_id");

                entity.Property(e => e.Gender1).HasColumnName("gender");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId)
                    .HasColumnType("INT")
                    .ValueGeneratedNever()
                    .HasColumnName("genre_id");

                entity.Property(e => e.GenreName).HasColumnName("genre_name");
;
            });

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("keyword");

                entity.Property(e => e.KeywordId)
                    .HasColumnType("INT")
                    .ValueGeneratedNever()
                    .HasColumnName("keyword_id");

                entity.Property(e => e.KeywordName).HasColumnName("keyword_name");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("language");

                entity.Property(e => e.LanguageId)
                    .HasColumnType("INT")
                    .ValueGeneratedNever()
                    .HasColumnName("language_id");

                entity.Property(e => e.LanguageCode).HasColumnName("language_code");

                entity.Property(e => e.LanguageName).HasColumnName("language_name");
            });

            modelBuilder.Entity<LanguageRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("language_role");

                entity.Property(e => e.RoleId)
                    .HasColumnType("INT")
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

                entity.Property(e => e.LanguageRole1).HasColumnName("language_role");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.MovieId)
                    .ValueGeneratedNever()
                    .HasColumnName("movie_id");

                entity.Property(e => e.Budget)
                    .HasColumnType("int")
                    .HasColumnName("budget");

                entity.Property(e => e.Homepage).HasColumnName("homepage");

                entity.Property(e => e.MovieStatus).HasColumnName("movie_status");

                entity.Property(e => e.Overview).HasColumnName("overview");

                entity.Property(e => e.Popularity).HasColumnName("popularity");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("date")
                    .HasColumnName("release_date");

                entity.Property(e => e.Revenue)
                    .HasColumnType("INT")
                    .HasColumnName("revenue");

                entity.Property(e => e.Runtime)
                    .HasColumnType("INT")
                    .HasColumnName("runtime");

                entity.Property(e => e.Tagline).HasColumnName("tagline");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.VoteAverage).HasColumnName("vote_average");

                entity.Property(e => e.VoteCount)
                    .HasColumnType("INT")
                    .HasColumnName("vote_count");

                entity.HasMany(a => a.Genres)
                       .WithMany(a => a.Movies)
                       .UsingEntity<MovieGenre>();

                entity.HasMany(a => a.Keywords)
                        .WithMany(a => a.Movies)
                        .UsingEntity<MovieKeyword>();
                entity.HasMany(a => a.Cast)
                    .WithMany(a => a.Movies)
                    .UsingEntity<MovieCast>();

            });

            modelBuilder.Entity<MovieCast>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.PersonId });

                entity.ToTable("movie_cast");

                entity.Property(e => e.CastOrder)
                    .HasColumnType("INT")
                    .HasColumnName("cast_order");

                entity.Property(e => e.CharacterName).HasColumnName("character_name");

                entity.Property(e => e.GenderId)
                    .HasColumnType("INT")
                    .HasColumnName("gender_id");

                entity.Property(e => e.MovieId)
                    .HasColumnType("INT")
                    .HasColumnName("movie_id");

                entity.Property(e => e.PersonId)
                    .HasColumnType("INT")
                    .HasColumnName("person_id");

                entity.HasOne(d => d.Gender)
                    .WithMany()
                    .HasForeignKey(d => d.GenderId);

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId);

                entity.HasOne(d => d.Person)
                    .WithMany()
                    .HasForeignKey(d => d.PersonId);
            });

            modelBuilder.Entity<MovieCompany>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("movie_company");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(10)")
                    .HasColumnName("company_id");

                entity.Property(e => e.MovieId)
                    .HasColumnType("int(10)")
                    .HasColumnName("movie_id");

                entity.HasOne(d => d.Company)
                    .WithMany()
                    .HasForeignKey(d => d.CompanyId);

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId);
            });

            modelBuilder.Entity<MovieCrew>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("movie_crew");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("int(10)")
                    .HasColumnName("department_id");

                entity.Property(e => e.Job)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("job");

                entity.Property(e => e.MovieId)
                    .HasColumnType("int(10)")
                    .HasColumnName("movie_id");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(10)")
                    .HasColumnName("person_id");

                entity.HasOne(d => d.Department)
                    .WithMany()
                    .HasForeignKey(d => d.DepartmentId);

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId);

                entity.HasOne(d => d.Person)
                    .WithMany()
                    .HasForeignKey(d => d.PersonId);
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.ToTable("movie_genres");

                entity.HasKey(e => new { e.MovieId, e.GenreId });

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                //entity.HasOne(d => d.Genre)
                //    .WithMany()
                //    .HasForeignKey(d => d.GenreId)
                //    .OnDelete(DeleteBehavior.ClientSetNull);

                //entity.HasOne(d => d.Movie)
                //    .WithMany()
                //    .HasForeignKey(d => d.MovieId)
                //    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MovieKeyword>(entity =>
            {
                entity.ToTable("movie_keywords");
                entity.HasKey(a => new { a.MovieId, a.KeywordId });
                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                //entity.HasOne(d => d.Keyword)
                //    .WithMany()
                //    .HasForeignKey(d => d.KeywordId)
                //    .OnDelete(DeleteBehavior.ClientSetNull);

                //entity.HasOne(d => d.Movie)
                //    .WithMany()
                //    .HasForeignKey(d => d.MovieId)
                //    .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<MovieLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("movie_languages");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.LanguageRoleId).HasColumnName("language_role_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Language)
                    .WithMany()
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.LanguageRole)
                    .WithMany()
                    .HasForeignKey(d => d.LanguageRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(10)")
                    .ValueGeneratedNever()
                    .HasColumnName("person_id");

                entity.Property(e => e.PersonName)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("person_name");
            });

            modelBuilder.Entity<ProductionCompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("production_company");

                entity.Property(e => e.CompanyId)
                    .HasColumnType("int(10)")
                    .ValueGeneratedNever()
                    .HasColumnName("company_id");

                entity.Property(e => e.CompanyName)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("company_name");
            });

            modelBuilder.Entity<ProductionCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("production_country");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(10)")
                    .HasColumnName("country_id");

                entity.Property(e => e.MovieId)
                    .HasColumnType("int(10)")
                    .HasColumnName("movie_id");

                entity.HasOne(d => d.Country)
                    .WithMany()
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
