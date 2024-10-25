using System;
using System.Collections.Generic;

namespace BillingSystem
{
    // Enum que define los estados posibles de un pedido
    public enum OrderStatus
    {
        Pendiente,
        Procesado,
        Enviado,
        Entregado,
        Cancelado
    }

    // Clase para gestionar el pedido
    public class OrderManagement
    {
        public string OrderID { get; private set; }      // ID único del pedido
        public DateTime OrderDate { get; private set; }  // Fecha de creación del pedido
        public OrderStatus Status { get; private set; }  // Estado del pedido
        public decimal TotalAmount { get; private set; } // Monto total del pedido
        private List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Lista de artículos del pedido

        // Constructor que inicializa el pedido con un ID y fecha actual, validando el ID
        public OrderManagement(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId) || orderId.Length < 5) 
                throw new ArgumentException("El ID del pedido debe ser único y contener al menos 5 caracteres.");

            OrderID = orderId;
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pendiente;  // Estado inicial del pedido
        }

        // Añade un producto al pedido, validando existencia y stock
        public void AddItem(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Producto no puede ser null.");
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor a cero.");

            if (!product.CheckStockAvailability(quantity))
                throw new InvalidOperationException($"Stock insuficiente para el producto {product.Name}.");

            // Agrega el producto al pedido y actualiza el monto total
            var orderItem = new OrderItem(product, quantity);
            OrderItems.Add(orderItem);
            TotalAmount += orderItem.ItemTotal;
            product.ReduceStock(quantity); // Reduce el stock del producto
        }

        // Actualiza el estado del pedido
        public void UpdateOrderStatus(OrderStatus status)
        {
            Status = status;
        }

        // Genera un resumen del pedido con un formato detallado
        public string GenerateOrderSummary()
        {
            var summary = GenerateHeader() + GenerateItemsDetail() + GenerateFooter();
            return summary;
        }

        // Método auxiliar para generar el encabezado del resumen del pedido
        private string GenerateHeader()
        {
            return $"Resumen del Pedido {OrderID}\nFecha: {OrderDate}\nEstado: {Status}\n";
        }

        // Método auxiliar para generar el detalle de cada artículo en el pedido
        private string GenerateItemsDetail()
        {
            string detail = "";
            foreach (var item in OrderItems)
            {
                detail += $"- {item.Product.Name} x{item.Quantity} = {item.ItemTotal:C}\n";
            }
            return detail;
        }

        // Método auxiliar para generar el total del pedido en el resumen
        private string GenerateFooter()
        {
            return $"Total del Pedido: {TotalAmount:C}\n";
        }
    }

    // Clase para representar un producto en el inventario
    public class Product
    {
        public string ProductID { get; private set; } // ID único del producto
        public string Name { get; private set; }      // Nombre del producto
        public decimal Price { get; private set; }    // Precio del producto
        public int Stock { get; private set; }        // Cantidad en stock del producto

        // Constructor que inicializa el producto y valida sus propiedades
        public Product(string productId, string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(productId) || productId.Length < 5) 
                throw new ArgumentException("El ID del producto debe ser único y contener al menos 5 caracteres.");
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price), "El precio debe ser positivo.");
            if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock), "El stock no puede ser negativo.");

            ProductID = productId;
            Name = name;
            Price = price;
            Stock = stock;
        }

        // Verifica si hay suficiente stock para la cantidad solicitada
        public bool CheckStockAvailability(int quantity)
        {
            return Stock >= quantity;
        }

        // Reduce el stock del producto después de validar la cantidad
        public void ReduceStock(int quantity)
        {
            if (quantity > Stock) throw new InvalidOperationException("Stock insuficiente.");
            Stock -= quantity;
        }
    }

    // Clase para representar un artículo dentro del pedido
    public class OrderItem
    {
        public Product Product { get; private set; } // Producto en el pedido
        public int Quantity { get; private set; }    // Cantidad solicitada del producto
        public decimal ItemTotal => Product.Price * Quantity; // Total del artículo (precio * cantidad)

        // Constructor que inicializa el artículo y valida existencia y cantidad
        public OrderItem(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Producto no puede ser null.");
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor a cero.");
            if (!product.CheckStockAvailability(quantity))
                throw new InvalidOperationException("Stock insuficiente para el producto.");

            Product = product;
            Quantity = quantity;
        }
    }
}
