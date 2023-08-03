using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FSFlightBuilder.Data.Models;

public partial class FSFBDbConn : DbContext
{
    private readonly string _connectionName;
    private readonly string _dataPath;

    public FSFBDbConn(string connectionName, string dataPath)
    {
        _connectionName = connectionName;
        _dataPath = dataPath;
    }

    public FSFBDbConn(DbContextOptions<FSFBDbConn> options, string connectionName, string dataPath)
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

    public virtual DbSet<Aircraft> Aircraft { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Comm> Comms { get; set; }

    public virtual DbSet<Navaid> Navaids { get; set; }

    public virtual DbSet<Parking> Parkings { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Runway> Runways { get; set; }

    public virtual DbSet<Waypoint> Waypoints { get; set; }
    public virtual DbSet<SqlMaster> SqlMasters { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlite("Data Source=.\\Database\\fsflightbuilder.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.Property(e => e.Airspeed).HasColumnType("DECIMAL");
            entity.Property(e => e.ClimbRate).HasColumnType("DECIMAL");
            entity.Property(e => e.ClimbSpeed).HasColumnType("DECIMAL");
            entity.Property(e => e.DescentRate).HasColumnType("DECIMAL");
            entity.Property(e => e.DescentSpeed).HasColumnType("DECIMAL");
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Name).HasColumnType("TEXT (255)");
            entity.Property(e => e.Texture).HasColumnType("TEXT (1024)");
            entity.Property(e => e.UIType)
                .HasColumnType("TEXT (50)")
                .HasColumnName("UIType");
            entity.Property(e => e.Unit).HasColumnType("CHAR");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => new { e.AirportId, e.FSType });

            entity.Property(e => e.AirportId).HasColumnType("TEXT (10)");
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Latitude).HasColumnType("DOUBLE");
            entity.Property(e => e.Longitude).HasColumnType("DOUBLE");
            entity.Property(e => e.MagVar).HasColumnType("DOUBLE");
        });

        modelBuilder.Entity<Comm>(entity =>
        {
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");

            entity.HasOne(d => d.Airport).WithMany(p => p.Comms)
                .HasForeignKey(d => new { d.AirportId, d.FSType })
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Navaid>(entity =>
        {
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Latitude).HasColumnType("DOUBLE");
            entity.Property(e => e.Longitude).HasColumnType("DOUBLE");
            entity.Property(e => e.MagVar).HasColumnType("DOUBLE");
            entity.Property(e => e.Name).HasColumnType("TEXT (255)");
            entity.Property(e => e.NavId).HasColumnType("TEXT (10)");
        });

        modelBuilder.Entity<Parking>(entity =>
        {
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Latitude).HasColumnType("DOUBLE");
            entity.Property(e => e.Longitude).HasColumnType("DOUBLE");

            entity.HasOne(d => d.Airport).WithMany(p => p.Parkings)
                .HasForeignKey(d => new { d.AirportId, d.FSType })
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            //entity.HasKey(e => new { e.Id });

            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");

            //entity.HasOne(d => d.Waypoint).WithMany(p => p.Routes)
            //    .HasForeignKey(d => new { d.WaypointId, d.FSType })
            //    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Runway>(entity =>
        {
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Heading).HasColumnType("DECIMAL");
            entity.Property(e => e.IlsHeading).HasColumnType("DECIMAL");
            entity.Property(e => e.Latitude).HasColumnType("DOUBLE");
            entity.Property(e => e.Longitude).HasColumnType("DOUBLE");

            entity.HasOne(d => d.Airport).WithMany(p => p.Runways)
                .HasForeignKey(d => new { d.AirportId, d.FSType })
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Waypoint>(entity =>
        {
            entity.Property(e => e.FSType)
                .HasColumnType("INT")
                .HasColumnName("FSType");
            entity.Property(e => e.Latitude).HasColumnType("DOUBLE");
            entity.Property(e => e.Longitude).HasColumnType("DOUBLE");
            entity.Property(e => e.MagVar).HasColumnType("DOUBLE");
        });

        modelBuilder.Entity<SqlMaster>(entity =>
        {
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
