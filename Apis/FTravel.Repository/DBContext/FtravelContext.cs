using System;
using System.Collections.Generic;
using FTravel.Repository.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FTravel.Repository.DBContext;

public partial class FtravelContext : DbContext
{
    public FtravelContext()
    {
    }

    public FtravelContext(DbContextOptions<FtravelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BusCompany> BusCompanies { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteStation> RouteStations { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceTicket> ServiceTickets { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripService> TripServices { get; set; }

    public virtual DbSet<TripTicketType> TripTicketTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BusCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BusCompa__3214EC07EE7118D4");

            entity.ToTable("BusCompany");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.ShortDescription).HasMaxLength(200);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC07C67B929F");

            entity.ToTable("City");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0767B5FC30");

            entity.ToTable("Customer");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.UnsignFullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07FB2BB517");

            entity.ToTable("Notification");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__48CFD27E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC077B4E0DAE");

            entity.ToTable("Order");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__CustomerI__06CD04F7");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0722A0C253");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ServiceName).HasMaxLength(150);
            entity.Property(e => e.Type).HasMaxLength(10);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__0C85DE4D");

            entity.HasOne(d => d.Ticket).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__OrderDeta__Ticke__0B91BA14");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07AB1D2960");

            entity.ToTable("Role");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Route__3214EC076D3E3823");

            entity.ToTable("Route");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.BusCompany).WithMany(p => p.Routes)
                .HasForeignKey(d => d.BusCompanyId)
                .HasConstraintName("FK__Route__BusCompan__4F7CD00D");

            entity.HasOne(d => d.EndPointNavigation).WithMany(p => p.RouteEndPointNavigations)
                .HasForeignKey(d => d.EndPoint)
                .HasConstraintName("FK__Route__EndPoint__4E88ABD4");

            entity.HasOne(d => d.StartPointNavigation).WithMany(p => p.RouteStartPointNavigations)
                .HasForeignKey(d => d.StartPoint)
                .HasConstraintName("FK__Route__StartPoin__4D94879B");
        });

        modelBuilder.Entity<RouteStation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RouteSta__3214EC07003F60C2");

            entity.ToTable("RouteStation");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteStations)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__RouteStat__Route__59063A47");

            entity.HasOne(d => d.Station).WithMany(p => p.RouteStations)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__RouteStat__Stati__59FA5E80");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC0758760C60");

            entity.ToTable("Service");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.ShortDescription).HasMaxLength(200);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.Services)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Service__RouteId__73BA3083");

            entity.HasOne(d => d.Station).WithMany(p => p.Services)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__Service__Station__74AE54BC");
        });

        modelBuilder.Entity<ServiceTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceT__3214EC07E7FD96DF");

            entity.ToTable("ServiceTicket");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTickets)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceTi__Servi__7D439ABD");

            entity.HasOne(d => d.Ticket).WithMany(p => p.ServiceTickets)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__ServiceTi__Ticke__7E37BEF6");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Station__3214EC076CCE1564");

            entity.ToTable("Station");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.BusCompany).WithMany(p => p.Stations)
                .HasForeignKey(d => d.BusCompanyId)
                .HasConstraintName("FK__Station__BusComp__5441852A");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3214EC0720D026DC");

            entity.ToTable("Ticket");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SeatCode).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeId)
                .HasConstraintName("FK__Ticket__TicketTy__6EF57B66");

            entity.HasOne(d => d.Trip).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__Ticket__TripId__6E01572D");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketTy__3214EC07BF501A49");

            entity.ToTable("TicketType");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.TicketTypes)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__TicketTyp__Route__6383C8BA");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC074E82A4E2");

            entity.ToTable("Transaction");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__Transacti__Walle__160F4887");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trip__3214EC07FAD9A29D");

            entity.ToTable("Trip");

            entity.Property(e => e.ActualEndDate).HasColumnType("datetime");
            entity.Property(e => e.ActualStartDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstimatedEndDate).HasColumnType("datetime");
            entity.Property(e => e.EstimatedStartDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UnsignName).HasMaxLength(100);
            entity.Property(e => e.OpenTicketDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.Trips)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Trip__RouteId__5EBF139D");
        });

        modelBuilder.Entity<TripService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TripServ__3214EC07FBCF2CC6");

            entity.ToTable("TripService");

            entity.HasOne(d => d.Service).WithMany(p => p.TripServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__TripServi__Servi__787EE5A0");

            entity.HasOne(d => d.Trip).WithMany(p => p.TripServices)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__TripServi__Servi__778AC167");
        });

        modelBuilder.Entity<TripTicketType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TripTick__3214EC07B3BF44EC");

            entity.ToTable("TripTicketType");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.TicketType).WithMany(p => p.TripTicketTypes)
                .HasForeignKey(d => d.TicketTypeId)
                .HasConstraintName("FK__TripTicke__Ticke__693CA210");

            entity.HasOne(d => d.Trip).WithMany(p => p.TripTicketTypes)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__TripTicke__TripI__68487DD7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07C0410F19");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ConfirmEmail).HasDefaultValue(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fcmtoken)
                .HasMaxLength(255)
                .HasColumnName("FCMToken");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.UnsignFullName).HasMaxLength(100);
            entity.Property(e => e.GoogleId).HasMaxLength(64);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__44FF419A");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07DC2620DF");

            entity.ToTable("Wallet");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.CustomerId)
                .HasConstraintName("FK__Wallet__Customer__114A936A");
        });

        //OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
