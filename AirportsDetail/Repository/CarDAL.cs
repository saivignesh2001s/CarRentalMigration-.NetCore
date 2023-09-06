namespace CarRental.Repository
{
    using CarRental.context;
    using CarRental.Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DAL
    {
        public class CarDAL:ICarDAL
        {
            private readonly DbContextclass context;
            public CarDAL(DbContextclass _context)
            {
                context = _context;
            }
            public List<Cars> getcar()
            {
                List<Cars> cars = context.Car.ToList();
                return cars;
            }
            public bool addcar(Cars c)
            {
                try
                {
                    context.Car.Add(c);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public Cars find(int id)
            {
                List<Cars> cars = context.Car.ToList();
                Cars c = cars.Find(x => x.Carid == id);
                return c;
            }
            public bool delete(int id)
            {
                try
                {
                    List<Cars> cars = context.Car.ToList();
                    Cars c = cars.Find(x => x.Carid == id);
                    context.Car.Remove(c);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public void update(int id, Cars c)
            {
                Cars k = context.Car.Find(id);
                k.Carid = c.Carid;
                k.Carname = c.Carname;
                k.PerDayCharge = c.PerDayCharge;
                k.ChargePerKm = c.ChargePerKm;
                k.Photo = c.Photo;
                k.Cartype = c.Cartype;
                k.Available = c.Available;
                context.SaveChanges();
            }
            public void locked(int id)
            {
                Cars k = context.Car.Find(id);
                k.Available = "No";
                context.SaveChanges();
            }
            public void unlocked(int id)
            {
                Cars k = context.Car.Find(id);
                k.Available = "Yes";
                context.SaveChanges();
            }
        }
    }

}
