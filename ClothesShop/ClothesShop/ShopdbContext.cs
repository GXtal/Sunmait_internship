using Microsoft.EntityFrameworkCore;

namespace ClothesShop;

public class ShopdbContext : DbContext
{
    public ShopdbContext()
    {
    }

    public ShopdbContext(DbContextOptions<ShopdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<CategoriesSection> CategoriesSections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=clothesshopdb;Username=postgres;Password=30122002vano");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {           
            entity.ToTable("brands");            

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");            
            entity.HasIndex(e => e.Name, "brands_name_key").IsUnique();
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.ToTable("sections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name"); 
            entity.HasIndex(e => e.Name, "sections_name_key").IsUnique();          
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();           
            
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");
            entity.HasOne(e => e.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_category_category_id");
        });

        modelBuilder.Entity<CategoriesSection>(entity =>
        {
            entity.ToTable("categories_sections");

            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.CaregoryId).HasColumnName("category_id");
            entity.HasKey(e => new { e.CaregoryId, e.SectionId });

            entity.HasOne(e => e.Section).WithMany(s => s.CategoriesSections)
                .HasForeignKey(e => e.SectionId)
                .HasConstraintName("fk_categories_sections_section_id");

            entity.HasOne(e => e.Category).WithMany(s => s.CategoriesSections)
                .HasForeignKey(e => e.CaregoryId)
                .HasConstraintName("fk_categories_sections_category_id");

        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
               .HasMaxLength(45)
               .HasColumnName("name");

            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.Property(e => e.Description)
               .HasMaxLength(200)
               .HasColumnName("description");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.HasOne(e => e.Category).WithMany(p => p.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_category_id");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.HasOne(e => e.Brand).WithMany(p => p.Products)
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_brand_id");

            
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);      
                        
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.PasswordHash)
               .HasMaxLength(64)
               .HasColumnName("password_hash");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.HasOne(e => e.Role).WithMany(p => p.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_role_id");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");           
            
            entity.Property(e => e.Surname)
                .HasMaxLength(45)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("contacts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(e => e.User).WithMany(p => p.Contacts)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("contacts_user_id_fkey");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");                     
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("addresses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(e => e.User).WithMany(p => p.Addresses)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("addresses_user_id_fkey");

            entity.Property(e => e.FullAddress)
                .HasMaxLength(200)
                .HasColumnName("full_address");                     
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("reviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("comment");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(e => e.User).WithMany(p => p.Reviews)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reviews_user_id");

            entity.Property(e => e.ProductId).HasColumnName("product_id");          
            entity.HasOne(e => e.Product).WithMany(p => p.Reviews)
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("fk_reviews_product_id");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);
                
            entity.Property(e => e.Path)
                .HasMaxLength(256)
                .HasColumnName("path");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.HasOne(e => e.Product).WithMany(p => p.Images)
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("fk_images_product_id");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("statuses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(e => e.User).WithMany(p => p.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_user_id");

            entity.Property(e => e.TotalCost)
                .HasColumnType("money")
                .HasColumnName("total_cost");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.ToTable("order_histories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.HasOne(e => e.Order).WithMany(p => p.OrderHistories)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_histories_order_id");
            
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.HasOne(e => e.Status).WithMany(p => p.OrderHistories)
               .HasForeignKey(e => e.StatusId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("fk_orders_histories_status_id");

            entity.Property(e => e.SetTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("set_time");
        });

        modelBuilder.Entity<OrdersProduct>(entity =>
        {
            entity.ToTable("orders_products");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.HasOne(e => e.Order).WithMany(p => p.OrdersProducts)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_products_order_id");

            entity.HasOne(e => e.Product).WithMany(p => p.OrdersProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_products_product_id");
        });

    }
}
