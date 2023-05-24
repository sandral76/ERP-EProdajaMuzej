using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class EProdajaMuzejContext : DbContext
{
    public EProdajaMuzejContext()
    {

    }

    public EProdajaMuzejContext(DbContextOptions<EProdajaMuzejContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetaljiPorudzbine> DetaljiPorudzbines { get; set; }

    public virtual DbSet<Izlozba> Izlozbas { get; set; }

    public virtual DbSet<IzlozbaUMuzeju> IzlozbaUMuzejus { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Korpa> Korpas { get; set; }

    public virtual DbSet<Muzej> Muzejs { get; set; }

    public virtual DbSet<Placanje> Placanjes { get; set; }

    public virtual DbSet<Porudzbina> Porudzbinas { get; set; }

    public virtual DbSet<StavkaPorudzbine> StavkaPorudzbines { get; set; }

    public virtual DbSet<TipKorisnika> TipKorisnikas { get; set; }

    public virtual DbSet<Ulaznica> Ulaznicas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Users;Trusted_Connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetaljiPorudzbine>(entity =>
        {
            entity.HasKey(e => e.DostavaId);

            entity.ToTable("DetaljiPorudzbine", "EProdajaMuzej");

            entity.Property(e => e.DostavaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("dostavaID");
            entity.Property(e => e.EmailDostave)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("email_dostave");
            entity.Property(e => e.Ime)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ime");
            entity.Property(e => e.KontaktTelefon)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kontakt_telefon");
            entity.Property(e => e.Prezime)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Izlozba>(entity =>
        {
            entity.ToTable("Izlozba", "EProdajaMuzej", tb => tb.HasTrigger("insert_ulaznica"));

            entity.Property(e => e.IzlozbaId)
                .HasDefaultValueSql("(NEXT VALUE FOR [EProdajaMuzej].[IzlozbaID])")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("izlozbaID");
            entity.Property(e => e.NazivIzlozbe)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("naziv_izlozbe");
            entity.Property(e => e.Umetnik)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("umetnik");
        });

        modelBuilder.Entity<IzlozbaUMuzeju>(entity =>
        {
            entity.HasKey(e => new { e.MuzejId, e.IzlozbaId });

            entity.ToTable("Izlozba_U_Muzeju", "EProdajaMuzej");

            entity.Property(e => e.MuzejId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("muzejID");
            entity.Property(e => e.IzlozbaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("izlozbaID");
            entity.Property(e => e.DatumOdrzavanja)
                .HasColumnType("datetime")
                .HasColumnName("datum_odrzavanja");
            entity.Property(e => e.Galerija)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("galerija");

            entity.HasOne(d => d.Izlozba).WithMany(p => p.IzlozbaUMuzejus)
                .HasForeignKey(d => d.IzlozbaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Izlozba_U_Muzeju_Izlozba");

            entity.HasOne(d => d.Muzej).WithMany(p => p.IzlozbaUMuzejus)
                .HasForeignKey(d => d.MuzejId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Izlozba_U_Muzeju_Muzej");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.ToTable("Korisnik", "EProdajaMuzej");

            entity.Property(e => e.KorisnikId)
                .HasDefaultValueSql("(NEXT VALUE FOR [EProdajaMuzej].[KorisnikID])")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("korisnikID");
            entity.Property(e => e.BrojTel)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("broj_tel");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ime");
            entity.Property(e => e.KorisnickoIme)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("korisnicko_ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prezime");
            entity.Property(e => e.TipKorisnikaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("tipKorisnikaID");

            entity.HasOne(d => d.TipKorisnika).WithMany(p => p.Korisniks)
                .HasForeignKey(d => d.TipKorisnikaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Korisnik_TipKorisnika");
        });

        modelBuilder.Entity<Korpa>(entity =>
        {
            entity.ToTable("Korpa", "EProdajaMuzej");

            entity.Property(e => e.KorpaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("korpaID");
            entity.Property(e => e.BrojUlaznica)
                .HasColumnType("numeric(4, 0)")
                .HasColumnName("broj_ulaznica");
            entity.Property(e => e.UkupanIznos)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("ukupan_iznos");
            entity.Property(e => e.ClientSecret)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("clientSecret");
            entity.Property(e => e.PaymentIntendId)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("paymentIntendId");
        });

        modelBuilder.Entity<Muzej>(entity =>
        {
            entity.ToTable("Muzej", "EProdajaMuzej");

            entity.Property(e => e.MuzejId)
                .HasDefaultValueSql("(NEXT VALUE FOR [EProdajaMuzej].[MuzejID])")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("muzejID");
            entity.Property(e => e.BrojTelefona)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("broj_telefona");
            entity.Property(e => e.Direktor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direktor");
            entity.Property(e => e.Grad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("grad");
            entity.Property(e => e.Naziv)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("naziv");
            entity.Property(e => e.Ulica)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ulica");
            entity.Property(e => e.VebSajt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("veb_sajt");
        });

        modelBuilder.Entity<Placanje>(entity =>
        {
            entity.ToTable("Placanje", "EProdajaMuzej");

            entity.HasIndex(e => e.PorudzbinaId, "UC_Placanje_Porudzbina").IsUnique();

            entity.Property(e => e.PlacanjeId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("placanjeID");
            entity.Property(e => e.BrojRacuna)
                .HasColumnType("numeric(30, 0)")
                .HasColumnName("broj_racuna");
            entity.Property(e => e.DatumPlacanja)
                .HasColumnType("datetime")
                .HasColumnName("datum_placanja");
            entity.Property(e => e.PorudzbinaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("porudzbinaID");
            entity.Property(e => e.TipPlacanja)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tip_placanja");
            entity.Property(e => e.VlasnikKartice)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vlasnik_kartice");

            entity.HasOne(d => d.Porudzbina).WithOne(p => p.Placanje)
                .HasForeignKey<Placanje>(d => d.PorudzbinaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Placanje_Porudzbina");
        });

        modelBuilder.Entity<Porudzbina>(entity =>
        {
            entity.ToTable("Porudzbina", "EProdajaMuzej", tb => tb.HasTrigger("update_uloga"));

            entity.Property(e => e.PorudzbinaId)
                .HasDefaultValueSql("(NEXT VALUE FOR [EProdajaMuzej].[PorudzbinaID])")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("porudzbinaID");
            entity.Property(e => e.DatumAzuriranja)
                .HasColumnType("datetime")
                .HasColumnName("datum_azuriranja");
            entity.Property(e => e.DatumKreiranja)
                .HasColumnType("datetime")
                .HasColumnName("datum_kreiranja");
            entity.Property(e => e.DostavaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("dostavaID");
            entity.Property(e => e.IznosPorudzbine)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("iznos_porudzbine");
            entity.Property(e => e.KorisnikId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("korisnikID");
            entity.Property(e => e.PopustNaPorudzbinu)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("popust_na_porudzbinu");
            entity.Property(e => e.StatusPorudzbine)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status_porudzbine");
                entity.Property(e => e.PaymentIntendId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("paymentIntendId");

            entity.HasOne(d => d.Dostava).WithMany(p => p.Porudzbinas)
                .HasForeignKey(d => d.DostavaId)
                .HasConstraintName("FK_Porudzbina_DetaljiPorudzbine");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Porudzbinas)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Porudzbina_Korisnik");
        });

        modelBuilder.Entity<StavkaPorudzbine>(entity =>
        {
            entity.HasKey(e => new { e.StavkaPorudzbineId, e.UlaznicaId });

            entity.ToTable("StavkaPorudzbine", "EProdajaMuzej", tb => tb.HasTrigger("update_korpa"));

            entity.HasIndex(e => e.UlaznicaId, "UC_StavkaPorudzbine_Ulaznica").IsUnique();

            entity.Property(e => e.StavkaPorudzbineId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("stavkaPorudzbineID");
            entity.Property(e => e.UlaznicaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("ulaznicaID");
            entity.Property(e => e.CenaStavka)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("cena_stavka");
            entity.Property(e => e.KorpaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("korpaID");
            entity.Property(e => e.PopustStavka)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("popust_stavka");
            entity.Property(e => e.PorudzbinaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("porudzbinaID");

            entity.HasOne(d => d.Korpa).WithMany(p => p.StavkaPorudzbines)
                .HasForeignKey(d => d.KorpaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkaPorudzbine_Korpa");

            entity.HasOne(d => d.Porudzbina).WithMany(p => p.StavkaPorudzbines)
                .HasForeignKey(d => d.PorudzbinaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkaPorudzbine_Porudzbina");

            /*entity.HasOne(d => d.Ulaznica).WithOne(p => p.StavkaPorudzbine)
                .HasForeignKey<StavkaPorudzbine>(d => d.UlaznicaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkaPorudzbine_Ulaznica");*/
        });

        modelBuilder.Entity<TipKorisnika>(entity =>
        {
            entity.ToTable("TipKorisnika", "EProdajaMuzej");

            entity.Property(e => e.TipKorisnikaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("tipKorisnikaID");
            entity.Property(e => e.Uloga)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("uloga");
        });

        modelBuilder.Entity<Ulaznica>(entity =>
        {
            entity.ToTable("Ulaznica", "EProdajaMuzej");

            entity.Property(e => e.UlaznicaId)
                .HasDefaultValueSql("(NEXT VALUE FOR [EProdajaMuzej].[UlaznicaID])")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("ulaznicaID");
            entity.Property(e => e.CenaUlaznice)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("cena_ulaznice");
            entity.Property(e => e.IzlozbaId)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("izlozbaID");
            entity.Property(e => e.Dostupna)
                .HasColumnType("bool")
                .HasColumnName("dostupna");

            entity.HasOne(d => d.Izlozba).WithMany(p => p.Ulaznicas)
                .HasForeignKey(d => d.IzlozbaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ulaznica_Izlozba");
        });
        modelBuilder.HasSequence<decimal>("IzlozbaID", "EProdajaMuzej")
            .HasMin(1L)
            .HasMax(999999999999999999L);
        modelBuilder.HasSequence<decimal>("KorisnikID", "EProdajaMuzej")
            .HasMin(1L)
            .HasMax(999999999999999999L);
        modelBuilder.HasSequence<decimal>("MuzejID", "EProdajaMuzej")
            .HasMin(1L)
            .HasMax(999999999999999999L);
        modelBuilder.HasSequence<decimal>("PorudzbinaID", "EProdajaMuzej")
            .HasMin(1L)
            .HasMax(999999999999999999L);
        modelBuilder.HasSequence<decimal>("UlaznicaID", "EProdajaMuzej")
            .HasMin(1L)
            .HasMax(999999999999999999L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
