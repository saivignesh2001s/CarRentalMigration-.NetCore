namespace CarRental.Repository
{
    using CarRental.context;
    using CarRental.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DAL
    {
        public class CustomerDAL : ICustomerDAL
        {
            private readonly DbContextclass context;
            public CustomerDAL(DbContextclass _context)
            {
                context = _context;
            }
            public List<Customers> GetCustomers()
            {
                return context.Customer.ToList();
            }
            public Customers GetCustomer(int id)
            {
                return context.Customer.Find(id);
            }
            public bool AddCustomer(Customers c)
            {
                try
                {
                    context.Customer.Add(c);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public bool updatecustomer(int id, Customers c)
            {
                try
                {
                    Customers k = context.Customer.Find(id);
                    k.Customerid = c.Customerid;
                    k.CustomerName = c.CustomerName;
                    k.mail = c.mail;
                    k.LoyaltyPoints = c.LoyaltyPoints;

                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public bool deletecustomer(int id)
            {
                try
                {
                    Customers k = context.Customer.Find(id);
                    context.Customer.Remove(k);
                    context.SaveChanges();
                    return true;

                }
                catch
                {
                    return false;
                }
            }
            public void Addloyalty(int km, int id)
            {
                Customers k = context.Customer.Find(id);
                k.LoyaltyPoints += km / 50;
                context.SaveChanges();
            }
            public void minusloyalty(int id)
            {
                Customers k = context.Customer.Find(id);
                k.LoyaltyPoints = k.LoyaltyPoints - 25;
                context.SaveChanges();
            }
        }
           
    }

}
