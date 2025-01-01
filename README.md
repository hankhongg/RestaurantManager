Download and execute the queries from: https://drive.google.com/drive/u/3/folders/1HEqQGUAwNZXqdmre0_Y-Msi37Jn23qGR
  QLNH -> QLNH_DML

Read and install necessary components/ packages in the PACKAGES_INSTALL

Change the connection string in RestaurantManager/Models/Database/QlnhContext.cs:
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=hankhongg-LAPTOP;Database=QLNH;Trusted_Connection=True;TrustServerCertificate=true;");

Server=hankhongg-LAPTOP;Database=QLNH;Trusted_Connection=True;TrustServerCertificate=true; => your connection string.

Watch demo at https://youtu.be/LKLc6m0k8PU