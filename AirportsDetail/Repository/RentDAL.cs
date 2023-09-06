using CarRental.context;
using CarRental.Model;
using CarRental.Repository.DAL;

namespace CarRental.Repository
{
    public class RentDAL:IRentDAL
    {
        private readonly DbContextclass context;
        private readonly ICarDAL cdal;

        public RentDAL(DbContextclass _context,ICarDAL dal)
        {
            context = _context;
            cdal= dal;

        }
        public bool rent(CarRents r)
        {
            try
            {
                context.CarRent.Add(r);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Return(int id, CarRents rent)
        {
            List<CarRents> rents = context.CarRent.ToList();
            CarRents r = rents.Find(x => x.RentId == id);
            context.CarRent.Remove(r);
            context.SaveChanges();
            context.CarRent.Add(rent);
            context.SaveChanges();

        }
        public void Cancel(int id)
        {
            List<CarRents> rents = context.CarRent.ToList();
            CarRents r = rents.Find(x => x.RentId == id);
            context.CarRent.Remove(r);
            context.SaveChanges();
        }
        public CarRents find(int id)
        {
            List<CarRents> rents = context.CarRent.ToList();
            CarRents r = rents.Find(x => x.RentId == id);
            return r;
        }
        public List<CarRents> rentlist()
        {
            return context.CarRent.ToList();
        }

        public Tuple<int, double> charges(CarRents r)
        {
            double charge = 0;
            
            DateTime d = Convert.ToDateTime(r.ReturnDate);
            DateTime d1 = Convert.ToDateTime(r.RentOrderDate);
            TimeSpan s = d - d1;
            int day = s.Days;
            Cars c = cdal.find(Convert.ToInt32(r.Carid));
            int charge1 = 0;
            if (day > 0)
            {
                charge1 = Convert.ToInt32(c.PerDayCharge) * day;
            }
            var kms = r.ReturnOdoReading - r.OdoReading;
            int km = Convert.ToInt32(kms);
            charge = Convert.ToInt32(c.ChargePerKm) * km;
            string typ = c.Cartype;
            double type = 0;
            switch (typ)
            {
                case "Luxury":
                    type = 0.5;
                    break;

                case "SUV":
                    type = 0.4;
                    break;
                case "Suden":
                    type = 0.3;
                    break;
                case "Compact":
                    type = 0.2;
                    break;
            }
            charge = charge * type;
            charge = charge + charge1;

            Tuple<int, double> k1 = Tuple.Create(km, charge);
            return k1;
        }
    }
}

