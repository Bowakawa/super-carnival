using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication2.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    [Table("orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("client_id")]
        public long ClientId { get; set; }

        // Добавьте это навигационное свойство
        [ForeignKey("ClientId")]
        public Client Client { get; set; }  // <-- Вот это исправляет ошибку!

        [InverseProperty("Order")]
        public ICollection<Box> Boxes { get; set; } = new List<Box>();

        public decimal TotalCost { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime DeliveryDate { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        CANCELED,
        PAID,
        NOT_PAID,
        IN_DELIVERY,
        DELIVERED
    }

    public class Box
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("Boxes")]
        public Order Order { get; set; } 

        [InverseProperty("Box")]
        public ICollection<BoxFlower> Flowers { get; set; }

        public int QuantityOfFlowers { get; set; }
        public double Weight { get; set; }
    }

    [Table("box_flowers")]
    public class BoxFlower
    {
        [Column("box_id")]
        public long BoxId { get; set; }

        [Column("flower_id")]
        public long FlowerId { get; set; }

        [ForeignKey("BoxId")]
        [InverseProperty("Flowers")]
        public Box Box { get; set; } = new Box();

        [ForeignKey("FlowerId")]
        public Flower Flower { get; set; } 
    }

    public class Flower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public double Weight { get; set; }
        public DateTime ExpirationDate { get; set; }

        [Column("price_per_package")]
        public decimal PricePerPackage { get; set; }

        [Column("dye_id")]
        public long DyeId { get; set; }

        [ForeignKey("DyeId")]
        public Dye Dye { get; set; } 

        [InverseProperty("Flowers")]
        public ICollection<Ingredient> Ingredients { get; set; }
    }

    public class Dye
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // GEL or POWDER
        public int Quantity { get; set; }
    }

    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public int Quantity { get; set; }

        [Column("price_per_package")]
        public decimal PricePerPackage { get; set; }
        public DateTime ExpirationDate { get; set; }

        [InverseProperty("Ingredients")]
        public ICollection<Flower> Flowers { get; set; }
    }
}
