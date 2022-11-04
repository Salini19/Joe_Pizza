using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Joe_Pizza.Models
{
    public partial class Pizza
    {
        public Pizza()
        {
            Orders = new HashSet<Order>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double? ProductPrice { get; set; }
        public string ProductDescription { get; set; } = null!;
        public string? ProductImage { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public bool AddPizza(Pizza pizza1)
        {
            try
            {
                Joe_PizzaContext context = new Joe_PizzaContext();
                context.Pizzas.Add(pizza1);
                context.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;

            }
        }

        public bool AvailabilityCheck()
        {

            Joe_PizzaContext context = new Joe_PizzaContext();
            List<Pizza> Pizzas = context.Pizzas.ToList();
            if (Pizzas.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool CheckPizza(int? id)
        {
            bool ans = false;
           
                Joe_PizzaContext context = new Joe_PizzaContext();
                List<Pizza> list = context.Pizzas.ToList();
                Pizza? p=list.Find(x => x.ProductId == id);
                if (p !=null)
                {
                    ans = true;
                }
            return ans;
            
        }
    }
}
