using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FSFlightBuilder.Data.Models;

public partial class NDRDbConn : DbContext
{
    private readonly string _connectionName;
    private readonly string _dataPath;

    public NDRDbConn(string connectionName, string dataPath)
    {
        _connectionName = connectionName;
        _dataPath = dataPath;
    }

    public NDRDbConn(DbContextOptions<NDRDbConn> options, string connectionName, string dataPath)
        : base(options)
    {
        _connectionName = connectionName;
        _dataPath = dataPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var conn = ConfigurationManager.ConnectionStrings[_connectionName];
        var connstring = conn.ConnectionString.Replace("|DataDirectory|", $"{_dataPath}\\");
        optionsBuilder.UseSqlite(connstring);
    }

    public virtual DbSet<DbAirport> Airports { get; set; }

    public virtual DbSet<DbComm> Comms { get; set; }

    public virtual DbSet<DbVor> Vors { get; set; }

    public virtual DbSet<DbNdb> Ndbs { get; set; }

    public virtual DbSet<DbParking> Parkings { get; set; }

    public virtual DbSet<DbRunway> Runways { get; set; }

    public virtual DbSet<DbRunwayEnd> RunwayEnds { get; set; }

    public virtual DbSet<DbILS> ILSs { get; set; }

    public virtual DbSet<DbWaypoint> Waypoints { get; set; }

    public virtual DbSet<DbAirway> Airways { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlite("Data Source=.\\Database\\fsflightbuilder.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbAirport>(entity =>
        {
            entity.HasKey(e => new { e.airport_id });

            entity.Property(e => e.laty).HasColumnType("DOUBLE");
            entity.Property(e => e.lonx).HasColumnType("DOUBLE");
            entity.Property(e => e.mag_var).HasColumnType("DOUBLE");
        });

        //modelBuilder.Entity<DbRunwayEnd>(entity =>
        //{
        //    entity.HasKey(e => new { e.runway_end_id });

        //});

        modelBuilder.Entity<DbComm>(entity =>
        {
            entity.HasKey(e => new { e.com_id });

            entity.HasOne(d => d.Airport).WithMany(p => p.Comms)
                .HasForeignKey(d => new { d.airport_id })
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DbVor>(entity =>
        {
            entity.HasKey(e => new { e.vor_id });

            entity.Property(e => e.laty).HasColumnType("DOUBLE");
            entity.Property(e => e.lonx).HasColumnType("DOUBLE");
            entity.Property(e => e.mag_var).HasColumnType("DOUBLE");

            //entity.HasOne(d => d.Airport).WithMany(p => p.VORs)
            //    .HasForeignKey(d => new { d.airport_id })
            //    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DbNdb>(entity =>
        {
            entity.HasKey(e => new { e.ndb_id });

            entity.Property(e => e.laty).HasColumnType("DOUBLE");
            entity.Property(e => e.lonx).HasColumnType("DOUBLE");
            entity.Property(e => e.mag_var).HasColumnType("DOUBLE");

            //entity.HasOne(d => d.Airport).WithMany(p => p.NDBs)
            //    .HasForeignKey(d => new { d.airport_id })
            //    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DbParking>(entity =>
        {
            entity.HasKey(e => new { e.parking_id });

            entity.Property(e => e.laty).HasColumnType("DOUBLE");
            entity.Property(e => e.lonx).HasColumnType("DOUBLE");

            entity.HasOne(d => d.Airport).WithMany(p => p.Parkings)
                .HasForeignKey(d => new { d.airport_id })
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DbAirway>(entity =>
        {
            entity.HasKey(e => new { e.airway_id });

            //entity.Property(e => e.FSType)
            //    .HasColumnType("INT")
            //    .HasColumnName("FSType");

            //entity.HasOne(d => d.Waypoint).WithMany(p => p.Routes)
            //    .HasForeignKey(d => new { d.waypoint_id })
            //    .OnDelete(DeleteBehavior.Cascade);
        });

        //modelBuilder.Entity<DbRunway>()
        //        .HasOne(e => e.RunwayEnd)
        //        .WithOne(e => e.Runway);
        //.HasForeignKey<DbRunwayEnd>(e => e.runway_end_id);

        //modelBuilder.Entity<DbRunwayEnd>()
        //        .HasOne(e => e.Runway)
        //        .WithOne(e => e.RunwayEnd)
        //        .HasForeignKey<DbRunway>(e => e.primary_end_id);

        //modelBuilder.Entity<DbRunwayEnd>()
        //        .HasOne(e => e.Runway)
        //        .WithOne(e => e.RunwayEnd)
        //        .HasForeignKey<DbRunway>(e => e.secondary_end_id);

        //modelBuilder.Entity<DbILS>()
        //        .HasOne(e => e.RunwayEnd)
        //        .WithOne(e => e.ILS)
        //        .HasForeignKey<DbRunwayEnd>(e => e.runway_end_id);

        modelBuilder.Entity<DbRunway>(entity =>
        {
            entity.HasKey(e => new { e.runway_id });

            entity.HasOne(d => d.Airport).WithMany(p => p.Runways)
                .HasForeignKey(d => new { d.airport_id })
                .OnDelete(DeleteBehavior.Cascade);
            //entity.HasOne(d => d.Airport).WithMany(p => p.Runways)
            //    .HasForeignKey(d => new { d.airport_id })
            //    .OnDelete(DeleteBehavior.Cascade);
        });

        //modelBuilder.Entity<DbRunwayEnd>(entity =>
        //{
        //    entity.HasKey(e => new { e.runway_end_id });

        //    entity.HasOne(d => d.Runway).WithMany(p => p.RunwayEnds)
        //        .HasForeignKey(d => new { d.runway_end_id })
        //        .OnDelete(DeleteBehavior.Cascade);
        //    entity.HasOne(d => d.ILS).WithMany(p => p.RunwayEnds)
        //        .HasForeignKey(d => new { d.runway_end_id })
        //        .OnDelete(DeleteBehavior.Cascade);
        //});

        //modelBuilder.Entity<DbILS>(entity =>
        //{
        //    entity.HasKey(e => new { e.ils_id });

        //    entity.HasOne(d => d.RunwayEnds).WithMany(p => p.ILSs)
        //        .HasForeignKey(d => new { d.runway_end_id })
        //        .OnDelete(DeleteBehavior.Cascade);


        //    //entity.HasOne(d => d.Runway).WithOne(e => e.RunwayEnd)
        //    //    .HasForeignKey<DbRunway>(d => d.primary_end_id)
        //    //    .OnDelete(DeleteBehavior.Cascade);
        //    //entity.HasOne(d => d.Runway).WithOne(e => e.RunwayEnd)
        //    //    .HasForeignKey<DbRunway>(d => d.secondary_end_id)
        //    //    .OnDelete(DeleteBehavior.Cascade);
        //    //entity.HasOne(d => d.ILS).WithOne(e => e.RunwayEnd)
        //    //    .HasForeignKey<DbILS>(d => d.loc_runway_end_id)
        //    //    .OnDelete(DeleteBehavior.Cascade);
        //});

        //modelBuilder.Entity<DbILS>(entity =>
        //{
        //    entity.HasKey(e => new { e.ils_id });
        //});

        //modelBuilder.Entity<DbILS>(entity =>
        //{
        //    entity.HasKey(e => new { e.ils_id });

        //    entity.HasOne(d => d.RunwayEnd).WithMany(p => p.ILSs)
        //        .HasForeignKey(d => new { d.loc_runway_end_id })
        //        .OnDelete(DeleteBehavior.Cascade);
        //});

        modelBuilder.Entity<DbWaypoint>(entity =>
        {
            entity.HasKey(e => new { e.waypoint_id });

            entity.Property(e => e.laty).HasColumnType("DOUBLE");
            entity.Property(e => e.lonx).HasColumnType("DOUBLE");
            entity.Property(e => e.mag_var).HasColumnType("DOUBLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
