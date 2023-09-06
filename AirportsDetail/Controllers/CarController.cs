using CarRental.Model;
using CarRental.Repository.DAL;
using CarRental.Repository;
using Microsoft.AspNetCore.Mvc;
using CarRental.context;
using CarRental.ModelforController;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly DbContextclass cd;
        private readonly IRentDAL rd;
        private readonly ICustomerDAL csdal;
        private readonly ICarDAL cdal;
        private readonly IHostEnvironment env;
        
      
      
        
        public CarController(DbContextclass cd,IRentDAL rd,ICustomerDAL csdal,ICarDAL cdal,IHostEnvironment env)
        {
            this.cdal = cdal;
            this.cd = cd;
            this.csdal = csdal;
            this.rd = rd;
            this.env = env;
        }
        public ActionResult EmpIndex()
        {
            List<CustModel> s = new List<CustModel>();
            List<Customers> s1 = csdal.GetCustomers();
            foreach (var item in s1)
            {
                CustModel m = new CustModel();
                m.Customerid = item.Customerid;
                m.CustomerName = item.CustomerName;
                m.Password = item.CustomerName;
                m.LoyaltyPoints = Convert.ToInt32(item.LoyaltyPoints);
                m.Email = item.mail.ToString();
                s.Add(m);

            }
            return View(s);
        }
        // GET: Car
        public ActionResult Index()
        {
            List<CAR> cars = new List<CAR>();

            List<Cars> cs = cdal.getcar();
            foreach (Cars c in cs)
            {
                CAR cd = new CAR();
                cd.Carid = c.Carid;
                cd.Carname = c.Carname;
                cd.PerDayCharge = c.PerDayCharge;
                cd.ChargePerKm = c.ChargePerKm;
                cd.CarType = c.Cartype;
                cd.Available = c.Available;
                cd.Photo = c.Photo;
                cars.Add(cd);
            }
            return View(cars);
        }

        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(IFormCollection c)
        {
            string k = c["Username"].ToString();
            string p = c["Password"].ToString();
            bool k1 = false;
            foreach (var item in cd.admin.ToList())
            {
                if (item.Username == k && item.Password == p)
                {
                    HttpContext.Session.SetString("u", item.Username);
                   // Session["u"] = k;
                    TempData["u2"] = k;
                    k1 = true;
                }
            }
            if (k1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Invalid Credentials..Try Again";
                return View();
            }

        }
        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            CAR c = new CAR();
            Cars cd = cdal.find(id);
            c.Carid = cd.Carid;
            c.Carname = cd.Carname;
            c.CarType = cd.Cartype;
            c.ChargePerKm = cd.ChargePerKm;
            c.PerDayCharge = cd.PerDayCharge;
            c.Available = cd.Available;
            c.Photo = cd.Photo;
            return View(c);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            CAR c = new CAR();
            c.Available = "Yes";
            return View(c);
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(CAR c)
        {
            try
            {
                Cars cd = new Cars();
                cd.Carid = c.Carid;
                cd.Carname = c.Carname;
                cd.PerDayCharge = c.PerDayCharge;
                cd.ChargePerKm = c.ChargePerKm;
                cd.Cartype = Request.Form["CarType"];
                cd.Available = c.Available;
                string k = "~/images/";
                cd.Photo = k + c.Photo;
                cdal.addcar(cd);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            Cars c = cdal.find(id);
            CAR c1 = new CAR();
            c1.Carid = c.Carid;
            c1.Carname = c.Carname;
            c1.ChargePerKm = c.ChargePerKm;
            c1.PerDayCharge = c.PerDayCharge;
            c1.CarType = c.Cartype;
            c1.Available = c.Available;
            c1.Photo = c.Photo;
            return View(c1);
        }

        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CAR c)
        {
            try
            {
                Cars cd = new Cars();
                cd.Carid = c.Carid;
                cd.Carname = c.Carname;
                cd.PerDayCharge = c.PerDayCharge;
                cd.ChargePerKm = c.ChargePerKm;
                cd.Cartype = c.CarType;
                cd.Available = c.Available;
                cd.Photo = c.Photo.ToString();
                cdal.update(id, cd);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            CAR c = new CAR();
            Cars cd = cdal.find(id);
            c.Carid = cd.Carid;
            c.Carname = cd.Carname;
            c.CarType = cd.Cartype;
            c.ChargePerKm = cd.ChargePerKm;
            c.PerDayCharge = cd.PerDayCharge;
            c.Available = cd.Available;
            c.Photo = cd.Photo;
            return View(c);
        }

        // POST: Car/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CAR c)
        {
            try
            {
                cdal.delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult OrderedRentals(int id)
        {
            List<CarRents> s = rd.rentlist();
            s = s.Where(x => x.Customerid == id).ToList();
            List<CARRENT> list = new List<CARRENT>();
            foreach (var rent in s)
            {
                CARRENT r = new CARRENT();
                r.RentId = rent.RentId;
                r.CarId = rent.Carid;
                r.CustomerId = rent.Customerid;
                r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
                r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
                r.LicenseNumber = rent.Licensenumber;


                list.Add(r);
            }
            return View(list);

        }
        public ActionResult preventBack()
        {
            string k = TempData["u2"].ToString();
            ViewBag.Message = "admin" + " " + k;
            HttpContext.Session.Clear();
           // Session["u"] = null;
            //Session.Abandon();
            return RedirectToAction("AdminLogin");
        }


    }
}
