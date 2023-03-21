using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EntityFC.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFC
{
    public class ShopContext: DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseMySql(@"server=localhost;port=3306;database=ShopDb;user=root;password=uxvjp967");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                        .HasKey(t => new { t.ProductId, t.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                        .HasOne(pc => pc.Product)
                        .WithMany(p => p.ProductCategories)
                        .HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductCategory>()
                       .HasOne(pc => pc.Category)
                       .WithMany(c => c.ProductCategories)
                       .HasForeignKey(pc => pc.CategoryId);
            modelBuilder.Entity<User>() // User tablosundaki Id değerini Fluent API ile PK yaptık.
                        .HasKey(c => c.Id);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<Adress> Adresses { get; set; }
        public Customer Customer { get; set; }
    }

    public class Adress
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string IdentifyNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxNumber { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new NorthwindContext())
            {
                // Tüm müşteri kayıtlarını getiriniz.

                // var customers = db.Customers.ToList();

                // foreach (var item in customers)
                // {
                //     Console.WriteLine(item.FirstName +" "+ item.LastName);
                // }

                // Tüm müşteri kayıtlarının sadece first_name ve last_name bilgilerini getiriniz.

                // var customers = db.Customers.Select(c=>new {
                //       c.FirstName,
                //       c.LastName  
                // });

                // foreach (var item in customers)
                // {
                //     Console.WriteLine(item.FirstName +" "+ item.LastName);
                // }


                // New York' da yaşayan müşterileri isim sırasına göre getiriniz.

                // var customers = db.Customers
                //                 .Where(i=>i.City == "New York")
                //                 .Select(s=> new {s.FirstName,s.LastName})
                //                 .ToList();

                // foreach (var item in customers)
                // {
                //     Console.WriteLine(item.FirstName +" "+ item.LastName);
                // }

                // "Beverages" kategorisine ait ürünlerin isimlerini getiriniz.

                // var productnames = db.Products
                //                 .Where(i=>i.Category=="Beverages")
                //                 .Select(i=>i.ProductName)
                //                 .ToList();

                // foreach (var name in productnames)
                // {
                //     Console.WriteLine(name);
                // }


                // En son eklenen 5 ürün bilgisini alınız.

                // var products = db.Products.OrderByDescending(i=>i.Id).Take(5);

                // foreach (var p in products)
                // {
                //     Console.WriteLine(p.ProductName);
                // }

                // Fiyatı 10 ile 30 arasında olan ürünlerin isim, fiyat bilgilerini azalan şekilde getiriniz.

                // var products = db.Products
                //                 .Where(i=> i.ListPrice>=10 && i.ListPrice<=30)
                //                 .Select(i=> new {
                //                      i.ProductName,
                //                      i.ListPrice  
                //                 }).ToList();


                // foreach (var item in products)
                // {
                //     Console.WriteLine(item.ProductName + " - " +item.ListPrice );
                // }


                // "Beverages" kategorisindeki ürünlerin ortalama fiyatı nedir?

                // var ortalama = db.Products
                //     .Where(i=>i.Category=="Beverages")
                //     .Average(i=>i.ListPrice);

                // Console.WriteLine("ortalama: {0}", ortalama);

                // "Beverages" kategorisinde kaç ürün vardır?

                // var adet = db.Products.Count(i=>i.Category=="Beverages");
                // Console.WriteLine("adet: {0}", adet);

                // "Beverages" veya "Condiments" kategorilerindeki ürünlerin toplam fiyatı nedir?

                // var toplam = db.Products
                // .Where(i=>i.Category == "Beverages" || i.Category=="Condiments")
                // .Sum(i=>i.ListPrice);

                // Console.WriteLine("toplam: {0}", toplam);

                // 'Tea' kelimesini içeren ürünleri getiriniz.

                // var products = db.Products
                //                 .Where(i=>i.ProductName.ToLower().Contains("Tea".ToLower()) || i.Description.Contains("Tea"))
                //                 .ToList();

                // foreach (var item in products)
                // {
                //     Console.WriteLine(item.ProductName);
                // }

                // En pahalı ürün ve en ucuz ürün hangisidir?

                var minPrice = db.Products.Min(i => i.ListPrice);
                var maxPrice = db.Products.Max(i => i.ListPrice);

                Console.WriteLine("min: {0} max: {1}", minPrice, maxPrice);


                var minproduct = db.Products
                .Where(i => i.ListPrice == (db.Products.Min(a => a.ListPrice)))
                .FirstOrDefault();

                Console.WriteLine($"name: {minproduct.ProductName} price: {minproduct.ListPrice}");


                var maxproduct = db.Products
                .Where(i => i.ListPrice == (db.Products.Max(a => a.ListPrice)))
                .FirstOrDefault();

                Console.WriteLine($"name: {maxproduct.ProductName} price: {maxproduct.ListPrice}");
            }
        }

        static void InsertUsers()
        {
            var users = new List<User>()
            {
                new User(){Username="erolemin", Email="erolemin@outlook.com.tr"},
                new User(){Username="zehranuracik", Email="zehranuracik@hotmail.com"},
                new User(){Username="aliyilmaz", Email="aliyilmaz@outlook.com"}
            };
            using(var db = new ShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }

        static void InsertAdresses()
        {
            var adresses = new List<Adress>()
            {
                new Adress(){FullName="Emin Erol", Title="Ev adresi", Body="Kocaeli"},
                new Adress(){FullName="Zehra Nur Acik", Title="Okul adresi", Body="Konya"},
                new Adress(){FullName="Hasan Cetin", Title="Is adresi", Body="Izmir"}
            };
            using(var db = new ShopContext())
            {
                db.Adresses.AddRange(adresses);
                db.SaveChanges();
            }
        }

        static void InsertCustomers()
        {
            using(var db = new ShopContext())
            {
                var customer = new Customer()
                {
                    IdentifyNumber = "412131",
                    FirstName = "Emin",
                    LastName = "Erol",
                    User = db.Users.FirstOrDefault(i => i.Id == 3)
                };
                db.Customers.Add(customer);
                db.SaveChanges();
            }
        }

        static void AddProducts()
        {
            using (var db = new ShopContext())
            {
                var products = new List<Product>();
                Console.Write("Enter the number of records: ");
                int pieces = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < pieces; i++)
                {
                    Console.Write($"{i + 1}. Product Name: ");
                    var productName = Console.ReadLine();
                    Console.Write("Price: ");
                    int price = Convert.ToInt32(Console.ReadLine());
                    products.Add(new Product { Name = productName, Price = price });
                }
                db.Products.AddRange(products);
                db.SaveChanges();
                Console.WriteLine("Veriler Eklendi!");
            }
        }
        static void GetAllProducts()
        {
            using (var db = new ShopContext())
            {
                var products = db.Products.ToList();
                foreach (var p in products)
                {
                    Console.WriteLine($"Name: {p.Name} = {p.Price}");
                }
            }
        }
        static void GetProductById(int id)
        {
            using (var context = new ShopContext())
            {
                var result = context.Products.Where(p => p.Id == id).FirstOrDefault();
                if (result != null)
                    Console.WriteLine($"Name: {result.Name} = {result.Price}");
            }
        }
        static void GetProductByName(string ad)
        {
            using (var context = new ShopContext())
            {
                var results = context.Products.Where(p => p.Name.ToLower().Contains(ad.ToLower())).ToList();
                foreach (var p in results)
                {
                    Console.WriteLine($"Name: {p.Name} = {p.Price}");
                }
            }
        }
        static void UpdateProduct(int id)
        {
            using (var db = new ShopContext())
            {
                var p = db.Products.Where(i => i.Id == id).FirstOrDefault();
                if (p != null)
                {
                    Console.Write("Enter the new price: ");
                    int newPrice = Convert.ToInt32(Console.ReadLine());
                    p.Price = newPrice;
                    db.SaveChanges();
                    Console.WriteLine("Was Uptaded!");
                }
            }
        }
        static void DeleteProduct(int id)
        {
            using (var db = new ShopContext())
            {
                var p = db.Products.FirstOrDefault(i => i.Id == id);
                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    Console.WriteLine("Record Deleted!");
                }
            }
        }
        static void AddProductCategory()
        {
            using(var db = new ShopContext())
            {
                var products = new List<Product>()
                {
                    new Product(){Name="iPhone 11", Price=12000},
                    new Product(){Name="iPhone 12", Price=15000}
                };
                db.Products.AddRange(products);
                var categories = new List<Category>()
                {
                    new Category(){Name="Telefon"},
                    new Category(){Name="Bilgisayar"}
                };
                db.Categories.AddRange(categories);

                int[] ids = new int[2] { 1, 2 };
                var p = db.Products.Find(1);
                p.ProductCategories = ids.Select(cid => new ProductCategory()
                {
                    CategoryId = cid,
                    ProductId = p.Id
                }).ToList();
                db.SaveChanges();
            }
        }
    }
}