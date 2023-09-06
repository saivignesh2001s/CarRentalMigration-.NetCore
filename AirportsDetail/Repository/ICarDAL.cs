using CarRental.Model;

namespace CarRental.Repository
{
    public interface ICarDAL
    {
        List<Cars> getcar();
        bool addcar(Cars c);
        Cars find(int id);
        bool delete(int id);
        void update(int id, Cars c);
        void locked(int id);
        void unlocked(int id);  
    }
}
