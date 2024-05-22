namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore.Metadata;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// The weather forecast database context.
    /// </summary>
    public class WeatherForecastDbContext : DbContext
    {
        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastDbContex"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the weather forecasts.
        /// </summary>
        /// <remarks>
        /// This <see cref="DbSet{WeatherForecast}"/> represents a collection of all <see cref="WeatherForecast"/> entities in the context, or that can be queried from the database. Entities can be added, modified, or removed, and these changes can be persisted back to the database.
        /// </remarks>
        public DbSet<WeatherForecast> WeatherForecasts
        {
            get;
            set;
        }

        /// <summary>
        /// Override the base method to further configure the model that was discovered by convention from the entity types exposed in <see cref="M:DbSet"/>DbSet<TEntity> properties on your derived context. The resulting model may be cached and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically define extension methods on this object that allow you to configure aspects of the model that are specific to a given database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeatherForecast>()
                .ToTable("WeatherForecasts", tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("ValidFrom").HasColumnName("ValidFrom");
                    ttb.HasPeriodEnd("ValidTo").HasColumnName("ValidTo");
                    ttb.UseHistoryTable("WeatherForecastsHistory");
                }));

            modelBuilder.Entity<WeatherForecast>()
                .Property<DateTime>("ValidFrom")
                .HasColumnName("ValidFrom")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<WeatherForecast>()
                .Property<DateTime>("ValidTo")
                .HasColumnName("ValidTo")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
