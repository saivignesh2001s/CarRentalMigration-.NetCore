using CarRental.Model;

namespace CarRental.Repository
{
    public interface ICustomerDAL
    {
        List<Customers> GetCustomers();
        Customers GetCustomer(int id);

        bool AddCustomer(Customers c);
        bool updatecustomer(int id, Customers c);   
        bool deletecustomer(int id);
        void Addloyalty(int km, int id);
        void minusloyalty(int id);
    }
}
