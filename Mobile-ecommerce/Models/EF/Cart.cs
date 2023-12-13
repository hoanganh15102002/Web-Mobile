using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.Price *Quantity;
    }
    public class Cart
    {       
        private List<CartItem> items = new List<CartItem>();     
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void AddItem(Product item, int soluong = 1)
        {
            var existingItem = items.FirstOrDefault(x => x.Product.ProductID == item.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity +=soluong;
            }
            else
            {
                items.Add(new CartItem {Product=item,Quantity=soluong});
            }
        }
        public void RemoveItem(int id)
        {
            var itemToRemove = items.FirstOrDefault(x => x.Product.ProductID == id);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }
        }
        public void UpdateItem(int id, int quantity)
        {
            var itemToUpdate = items.FirstOrDefault(x => x.Product.ProductID == id);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
            }
        }
        public decimal GetTotal()
        {
            return items.Sum(x => x.TotalPrice);
        }
        public int TotalQuantity()
        {
            return items.Sum(s => s.Quantity);
        }
        public void Clear()
        {
            items.Clear();
        }
    }
}