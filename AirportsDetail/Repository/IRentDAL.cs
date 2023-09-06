using CarRental.Model;

namespace CarRental.Repository
{
    public interface IRentDAL
    {
        bool rent(CarRents r);
        void Return(int id, CarRents r);
        void Cancel(int id);
        CarRents find(int id);
        List<CarRents> rentlist();
        Tuple<int,double> charges(CarRents r);
    }
}
