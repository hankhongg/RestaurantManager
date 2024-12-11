using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.Models;

public partial class QlnhContext : DbContext
{
    public QlnhContext()
    {
    }

    public QlnhContext(DbContextOptions<QlnhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRole> AccountRoles { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DiningTable> DiningTables { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<FinancialHistory> FinancialHistories { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Stockin> Stockins { get; set; }

    public virtual DbSet<StockinDetailsDrinkOther> StockinDetailsDrinkOthers { get; set; }

    public virtual DbSet<StockinDetailsIngre> StockinDetailsIngres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=hankhongg-LAPTOP;Database=QLNH;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccUsername).HasName("pk_username");

            entity.ToTable("ACCOUNT");

            entity.HasIndex(e => e.AccEmail, "uq_acc_email").IsUnique();

            entity.Property(e => e.AccUsername)
                .HasMaxLength(100)
                .HasColumnName("ACC_USERNAME");
            entity.Property(e => e.AccAddress)
                .HasMaxLength(100)
                .HasColumnName("ACC_ADDRESS");
            entity.Property(e => e.AccBday).HasColumnName("ACC_BDAY");
            entity.Property(e => e.AccDisplayname)
                .HasMaxLength(100)
                .HasColumnName("ACC_DISPLAYNAME");
            entity.Property(e => e.AccEmail)
                .HasMaxLength(100)
                .HasColumnName("ACC_EMAIL");
            entity.Property(e => e.AccGender)
                .HasMaxLength(5)
                .HasColumnName("ACC_GENDER");
            entity.Property(e => e.AccPassword)
                .HasMaxLength(100)
                .HasColumnName("ACC_PASSWORD");
            entity.Property(e => e.AccPhone)
                .HasMaxLength(20)
                .HasColumnName("ACC_PHONE");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ACCOUNT__ROLE_ID__6C190EBB");
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pk_id");

            entity.ToTable("ACCOUNT_ROLE");

            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName).HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BkId).HasName("pk_booking_id");

            entity.ToTable("BOOKING");

            entity.Property(e => e.BkId).HasColumnName("BK_ID");
            entity.Property(e => e.BkCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BK_CODE");
            entity.Property(e => e.BkOtime)
                .HasColumnType("datetime")
                .HasColumnName("BK_OTIME");
            entity.Property(e => e.BkStatus).HasColumnName("BK_STATUS");
            entity.Property(e => e.BkStime)
                .HasColumnType("datetime")
                .HasColumnName("BK_STIME");
            entity.Property(e => e.CusId).HasColumnName("CUS_ID");
            entity.Property(e => e.EmpId).HasColumnName("EMP_ID");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
            entity.Property(e => e.TabId).HasColumnName("TAB_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BOOKING__CUS_ID__4D94879B");

            entity.HasOne(d => d.Emp).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__BOOKING__EMP_ID__4CA06362");

            entity.HasOne(d => d.Tab).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TabId)
                .HasConstraintName("FK__BOOKING__TAB_ID__4E88ABD4");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CusId).HasName("pk_cus_id");

            entity.ToTable("CUSTOMER");

            entity.HasIndex(e => e.CusPhone, "uq_cus_phone").IsUnique();

            entity.Property(e => e.CusId).HasColumnName("CUS_ID");
            entity.Property(e => e.CusAddr)
                .HasMaxLength(100)
                .HasColumnName("CUS_ADDR");
            entity.Property(e => e.CusCccd)
                .HasMaxLength(12)
                .HasColumnName("CUS_CCCD");
            entity.Property(e => e.CusCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CUS_CODE");
            entity.Property(e => e.CusEmail)
                .HasMaxLength(50)
                .HasColumnName("CUS_EMAIL");
            entity.Property(e => e.CusName)
                .HasMaxLength(50)
                .HasColumnName("CUS_NAME");
            entity.Property(e => e.CusPhone)
                .HasMaxLength(20)
                .HasColumnName("CUS_PHONE");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
        });

        modelBuilder.Entity<DiningTable>(entity =>
        {
            entity.HasKey(e => e.TabId).HasName("pk_table_id");

            entity.ToTable("DINING_TABLE");

            entity.HasIndex(e => e.TabNum, "UQ__DINING_T__0E575E16E0578BA0").IsUnique();

            entity.Property(e => e.TabId).HasColumnName("TAB_ID");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
            entity.Property(e => e.TabNum).HasColumnName("TAB_NUM");
            entity.Property(e => e.TabStatus).HasColumnName("TAB_STATUS");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("pk_emp_id");

            entity.ToTable("EMPLOYEE");

            entity.HasIndex(e => e.EmpCccd, "uq_emp_cccd").IsUnique();

            entity.Property(e => e.EmpId).HasColumnName("EMP_ID");
            entity.Property(e => e.EmpAddr)
                .HasMaxLength(100)
                .HasColumnName("EMP_ADDR");
            entity.Property(e => e.EmpCccd)
                .HasMaxLength(12)
                .HasColumnName("EMP_CCCD");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EMP_CODE");
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .HasColumnName("EMP_NAME");
            entity.Property(e => e.EmpPhone)
                .HasMaxLength(20)
                .HasColumnName("EMP_PHONE");
            entity.Property(e => e.EmpRole)
                .HasMaxLength(50)
                .HasColumnName("EMP_ROLE");
            entity.Property(e => e.EmpSalary)
                .HasColumnType("money")
                .HasColumnName("EMP_SALARY");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
        });

        modelBuilder.Entity<FinancialHistory>(entity =>
        {
            entity.HasKey(e => e.FinId).HasName("PK__FINANCIA__20636AD9D4BC95B6");

            entity.ToTable("FINANCIAL_HISTORY");

            entity.Property(e => e.FinId).HasColumnName("FIN_ID");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.FinDate)
                .HasColumnType("datetime")
                .HasColumnName("FIN_DATE");
            entity.Property(e => e.ReferenceId).HasColumnName("REFERENCE_ID");
            entity.Property(e => e.ReferenceType)
                .HasMaxLength(20)
                .HasColumnName("REFERENCE_TYPE");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngreId).HasName("pk_ingre_id");

            entity.ToTable("INGREDIENTS");

            entity.Property(e => e.IngreId).HasColumnName("INGRE_ID");
            entity.Property(e => e.IngreCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INGRE_CODE");
            entity.Property(e => e.IngreName)
                .HasMaxLength(30)
                .HasColumnName("INGRE_NAME");
            entity.Property(e => e.IngrePrice)
                .HasDefaultValue(0m)
                .HasColumnType("money")
                .HasColumnName("INGRE_PRICE");
            entity.Property(e => e.InstockKg).HasColumnName("INSTOCK_KG");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("pk_dish_id");

            entity.ToTable("MENU_ITEMS");

            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.Instock)
                .HasDefaultValue(0.0)
                .HasColumnName("INSTOCK");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ITEM_CODE");
            entity.Property(e => e.ItemCprice)
                .HasColumnType("money")
                .HasColumnName("ITEM_CPRICE");
            entity.Property(e => e.ItemImg).HasColumnName("ITEM_IMG");
            entity.Property(e => e.ItemName)
                .HasMaxLength(50)
                .HasColumnName("ITEM_NAME");
            entity.Property(e => e.ItemSprice)
                .HasColumnType("money")
                .HasColumnName("ITEM_SPRICE");
            entity.Property(e => e.ItemType)
                .HasMaxLength(10)
                .HasColumnName("ITEM_TYPE");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("pk_receipt_id");

            entity.ToTable("RECEIPT");

            entity.Property(e => e.RecId).HasColumnName("REC_ID");
            entity.Property(e => e.CusId).HasColumnName("CUS_ID");
            entity.Property(e => e.EmpId).HasColumnName("EMP_ID");
            entity.Property(e => e.Isdeleted).HasColumnName("ISDELETED");
            entity.Property(e => e.RecCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REC_CODE");
            entity.Property(e => e.RecPay)
                .HasColumnType("money")
                .HasColumnName("REC_PAY");
            entity.Property(e => e.RecTime)
                .HasColumnType("datetime")
                .HasColumnName("REC_TIME");
            entity.Property(e => e.TabId).HasColumnName("TAB_ID");

            entity.HasOne(d => d.Cus).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.CusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__RECEIPT__CUS_ID__5441852A");

            entity.HasOne(d => d.Emp).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__RECEIPT__EMP_ID__534D60F1");

            entity.HasOne(d => d.Tab).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.TabId)
                .HasConstraintName("FK__RECEIPT__TAB_ID__5535A963");
        });

        modelBuilder.Entity<ReceiptDetail>(entity =>
        {
            entity.HasKey(e => new { e.RecId, e.ItemId }).HasName("PK__RECEIPT___396D79007ABBEB3C");

            entity.ToTable("RECEIPT_DETAILS");

            entity.Property(e => e.RecId).HasColumnName("REC_ID");
            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("PRICE");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");

            entity.HasOne(d => d.Item).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECEIPT_D__ITEM___59FA5E80");

            entity.HasOne(d => d.Rec).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.RecId)
                .HasConstraintName("FK__RECEIPT_D__REC_I__59063A47");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.IngreId }).HasName("PK__RECIPES__51D2FEEFD3FF8CE5");

            entity.ToTable("RECIPES");

            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.IngreId).HasColumnName("INGRE_ID");
            entity.Property(e => e.IngreQuantityKg).HasColumnName("INGRE_QUANTITY_KG");

            entity.HasOne(d => d.Ingre).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IngreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RECIPES__INGRE_I__49C3F6B7");

            entity.HasOne(d => d.Item).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__RECIPES__ITEM_ID__48CFD27E");
        });

        modelBuilder.Entity<Stockin>(entity =>
        {
            entity.HasKey(e => e.StoId).HasName("pk_stockin_id");

            entity.ToTable("STOCKIN");

            entity.Property(e => e.StoId).HasColumnName("STO_ID");
            entity.Property(e => e.StoCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STO_CODE");
            entity.Property(e => e.StoDate)
                .HasColumnType("datetime")
                .HasColumnName("STO_DATE");
            entity.Property(e => e.StoPrice)
                .HasColumnType("money")
                .HasColumnName("STO_PRICE");
        });

        modelBuilder.Entity<StockinDetailsDrinkOther>(entity =>
        {
            entity.HasKey(e => new { e.StoId, e.ItemId }).HasName("PK__STOCKIN___E3A93569E53CA331");

            entity.ToTable("STOCKIN_DETAILS_DRINK_OTHER");

            entity.Property(e => e.StoId).HasColumnName("STO_ID");
            entity.Property(e => e.ItemId).HasColumnName("ITEM_ID");
            entity.Property(e => e.Cprice)
                .HasColumnType("money")
                .HasColumnName("CPRICE");
            entity.Property(e => e.QuantityUnits).HasColumnName("QUANTITY_UNITS");
            entity.Property(e => e.TotalCprice)
                .HasDefaultValue(0m)
                .HasColumnType("money")
                .HasColumnName("TOTAL_CPRICE");

            entity.HasOne(d => d.Item).WithMany(p => p.StockinDetailsDrinkOthers)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__STOCKIN_D__ITEM___6383C8BA");

            entity.HasOne(d => d.Sto).WithMany(p => p.StockinDetailsDrinkOthers)
                .HasForeignKey(d => d.StoId)
                .HasConstraintName("FK__STOCKIN_D__STO_I__628FA481");
        });

        modelBuilder.Entity<StockinDetailsIngre>(entity =>
        {
            entity.HasKey(e => new { e.StoId, e.IngreId }).HasName("PK__STOCKIN___05599ABCBA6C2305");

            entity.ToTable("STOCKIN_DETAILS_INGRE");

            entity.Property(e => e.StoId).HasColumnName("STO_ID");
            entity.Property(e => e.IngreId).HasColumnName("INGRE_ID");
            entity.Property(e => e.Cprice)
                .HasColumnType("money")
                .HasColumnName("CPRICE");
            entity.Property(e => e.QuantityKg).HasColumnName("QUANTITY_KG");
            entity.Property(e => e.TotalCprice)
                .HasDefaultValue(0m)
                .HasColumnType("money")
                .HasColumnName("TOTAL_CPRICE");

            entity.HasOne(d => d.Ingre).WithMany(p => p.StockinDetailsIngres)
                .HasForeignKey(d => d.IngreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__STOCKIN_D__INGRE__5FB337D6");

            entity.HasOne(d => d.Sto).WithMany(p => p.StockinDetailsIngres)
                .HasForeignKey(d => d.StoId)
                .HasConstraintName("FK__STOCKIN_D__STO_I__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
