using CarRental.Model;
using CarRental.ModelforController;
using CarRental.Repository.DAL;
using CarRental.Repository;
using Microsoft.AspNetCore.Mvc;
using CarRental.context;
using Newtonsoft.Json;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerDAL cd;
        private readonly ICarDAL cdal;
        private readonly IRentDAL fs;
        private readonly DbContextclass fd;
       
        public CustomerController(ICustomerDAL cd,ICarDAL cdal,IRentDAL fs,DbContextclass fd)
        {
            this.cd = cd;
            this.cdal = cdal;
            this.fs = fs;
            this.fd = fd;
        }
        public ActionResult Register()
        {
            CustModel customer = new CustModel();
            Random k = new Random();
            customer.Customerid = k.Next(1000, 40000);
            customer.LoyaltyPoints = 0;
            return View(customer);
        }
        [HttpPost]
        public ActionResult Register(IFormCollection c)
        {

            Customers c1 = new Customers();
            c1.LoyaltyPoints = Convert.ToInt32(c["LoyaltyPoints"]);
            c1.Customerid = Convert.ToInt32(c["Customerid"]);
            c1.CustomerName = c["CustomerName"].ToString();
            c1.mail = c["Email"].ToString();
            c1.Password = c["Password"].ToString();
            bool k = cd.AddCustomer(c1);
            if (k)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }



        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(IFormCollection c)
        {
            string s = c["Email"].ToString();
            string k = c["Password"].ToString();
            bool k1 = false;
            foreach (var item in cd.GetCustomers())
            {
                if (item.mail == s && item.Password == k)
                {
                    TempData["User"] =JsonConvert.SerializeObject(item);
                    HttpContext.Session.SetString("u1", item.mail);
                  //  Session["u1"] = item;
                    k1 = true;
                }
            }
            if (k1)
            {
                //return RedirectToAction("Index");
                return RedirectToAction("Search");
            }
            else
            {
                ViewBag.Message = "Invalid Credentials..Try Again";
                return View();
            }


        }
        public ActionResult forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgotpassword(FormCollection c)
        {
            string k = c["Email"].ToString();
            string p = c["Password"].ToString();
            Customers c1 = null;
            foreach (var item in cd.GetCustomers())
            {
                if (k == item.mail)
                {
                    c1 = item;
                }

            }
            c1.Password = p;
            bool k1 = cd.updatecustomer(c1.Customerid, c1);
            if (k1)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }

        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(searchdates s)
        {
            DateTime k1 = new DateTime();
            DateTime k2 = new DateTime();
          //  Customers k = (Customers)TempData["user"];
            Customers k = JsonConvert.DeserializeObject<Customers>(TempData["user"].ToString());
            TempData["user"] = JsonConvert.SerializeObject(k);


            if (s.RentDate < DateTime.Today)
            {

                ViewBag.Message13 = "Check the date..";
            }
            else
            {
                k1 = Convert.ToDateTime(s.RentDate);
                TempData["Rentdate"] = k1;
            }


            if (s.ReturnDate < s.RentDate)
            {
                ViewBag.Message33 = "ReturnDate can not be more than rent date";
            }
            else
            {
                k2 = Convert.ToDateTime(s.ReturnDate);
                TempData["Returndate"] = k2;
            }
            List<CarRents> m1 = fs.rentlist();
            m1 = m1.Where(x => ((k1 <= x.ReturnDate) && (x.RentOrderDate <= k2)) && (x.Customerid == k.Customerid) && x.ReturnOdoReading is null).ToList();
            if (m1.Count != 0)
            {
                ViewBag.Message14 = "You have booked another car that day";
            }
            if (k1.Equals(s.RentDate) && k2.Equals(s.ReturnDate) && (m1.Count == 0))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public List<int> Carlist()
        {
            DateTime k1 = Convert.ToDateTime(TempData["RentDate"]);
            DateTime k2 = Convert.ToDateTime(TempData["ReturnDate"]);
            searchdates s = new searchdates();
            s.ReturnDate = k2;
            s.RentDate = k1;
            TempData["ReturnDate"] = k2;
            TempData["RentDate"] = k1;

            List<CarRents> m1 = fs.rentlist();
            m1 = m1.Where(x => (k1 <= x.ReturnDate && x.RentOrderDate <= k2) && x.ReturnOdoReading is null).ToList();
            List<int> m2 = new List<int>();
            foreach (var item in m1)
            {
                int k = Convert.ToInt32(item.Carid);
                m2.Add(k);
            }
            return m2;
        }

        // GET: Customer
        public ActionResult Index()
        {

            List<CAR> cars = new List<CAR>();

            List<Cars> cs1 = cdal.getcar();
            List<int> ln = Carlist();


            foreach (int item in ln)
            {
                Cars k = cdal.find(item);
                cs1.Remove(k);

            }
            foreach (var item in cs1)
            {
                cdal.unlocked(item.Carid);
                CAR k = new CAR();
                k.Carid = item.Carid;

                k.Carname = item.Carname;
                k.Available = item.Available;
                k.PerDayCharge = item.PerDayCharge;
                k.ChargePerKm = item.ChargePerKm;
                k.CarType = item.Cartype;
                k.Photo = item.Photo;
                cars.Add(k);
            }

            return View(cars);
        }

        // GET: Customer/Details/5
        public ActionResult Details()
        {


            Customers g = (Customers)TempData["User"];
            TempData["User"] = g;
            int id = g.Customerid;
            Customers k = cd.GetCustomer(id);

            CustModel k1 = new CustModel();
            k1.Customerid = k.Customerid;
            k1.CustomerName = k.CustomerName;
            k1.Password = k.Password;
            k1.LoyaltyPoints = Convert.ToInt32(k.LoyaltyPoints);
            k1.Email = k.mail;
            return View(k1);
        }



        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            Customers k = cd.GetCustomer(id);
            CustModel k1 = new CustModel();
            k1.Customerid = k.Customerid;
            k1.CustomerName = k.CustomerName;
            k1.LoyaltyPoints = Convert.ToInt32(k.LoyaltyPoints);
            k1.Email = k.mail;


            return View(k1);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Customers k = new Customers();
            k.Customerid = Convert.ToInt32(collection["Customerid"]);
            k.CustomerName = collection["CustomerName"].ToString();
            k.mail = collection["Email"].ToString();
            k.LoyaltyPoints = Convert.ToInt32(collection["LoyaltyPoints"]);

            bool k1 = cd.updatecustomer(id, k);
            if (k1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        public ActionResult Rent(int id)
        {
            Customers k = JsonConvert.DeserializeObject<Customers>(TempData["user"].ToString());

            //Customers k = (Customers)TempData["user"];
            CARRENT r = new CARRENT();
            Random k1 = new Random();
            r.RentId = k1.Next(1000, 40000);

            r.CustomerId = k.Customerid;
            r.CarId = id;
            Cars k2 = cdal.find(id);
            ViewBag.image = k2.Photo;
            TempData["user"] = JsonConvert.SerializeObject(k);

            r.RentOrderDate = Convert.ToDateTime(TempData["RentDate"]);
            r.ReturnDate = Convert.ToDateTime(TempData["ReturnDate"]);

            TempData["RentDate"] = r.RentOrderDate;
            TempData["ReturnDate"] = r.ReturnDate;
            return View(r);
        }
        [HttpPost]
        public ActionResult Rent(int id, CARRENT r2)
        {


            CarRents r = new CarRents();
            r.RentId = r2.RentId;
            r.Carid = r2.CarId;

            r.Customerid = r2.CustomerId;
            r.OdoReading = r2.OdoReading;
            r.Licensenumber = r2.LicenseNumber;

            r.RentOrderDate = Convert.ToDateTime(r2.RentOrderDate);


            r.ReturnDate = Convert.ToDateTime(r2.ReturnDate);

            r.ReturnOdoReading = null;

            bool k = fs.rent(r);
            if (k)
            {

                return RedirectToAction("Presenttrentals");
            }
            else
            {
                return View();
            }



        }
        public ActionResult RentNow(int id)
        {
            CarRents rent = fs.find(id);
            CARRENT r = new CARRENT();
            r.RentId = rent.RentId;
            r.CarId = rent.Carid;
            Cars k2 = cdal.find((int)rent.Carid);
            ViewBag.image = k2.Photo;
            r.CustomerId = rent.Customerid;
            r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
            r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
            r.OdoReading = rent.OdoReading;
            r.ReturnOdoReading = rent.ReturnOdoReading;
            r.LicenseNumber = rent.Licensenumber;
            return View(r);
        }
        [HttpPost]
        public ActionResult RentNow(int id, CARRENT rent)
        {
            try
            {
                CarRents r = new CarRents();
                r.RentId = rent.RentId;
                r.Carid = rent.CarId;
                r.Customerid = rent.CustomerId;
                r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
                r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
                r.OdoReading = rent.OdoReading;
                r.ReturnOdoReading = rent.ReturnOdoReading;
                r.Licensenumber = rent.LicenseNumber;
                fs.Return(id, r);
                return RedirectToAction("Presenttrentals");

            }
            catch
            {
                return View();
            }

        }
        public ActionResult Pastrentals()
        {
            List<CarRents> ls = fs.rentlist();
            Customers k = JsonConvert.DeserializeObject<Customers>(TempData["User"].ToString());
            int id = k.Customerid;
            TempData["user"] = JsonConvert.SerializeObject(k);
            ls = ls.Where(x => (x.ReturnDate < DateTime.Today || x.ReturnOdoReading != null) && x.Customerid == id).ToList();
            List<CARRENT> list = new List<CARRENT>();
            foreach (var rent in ls)
            {
                CARRENT r = new CARRENT();
                r.RentId = rent.RentId;
                r.CarId = rent.Carid;
                r.CustomerId = rent.Customerid;
                r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
                r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
                r.OdoReading = rent.OdoReading;
                r.ReturnOdoReading = rent.ReturnOdoReading;
                r.LicenseNumber = rent.Licensenumber;



                list.Add(r);
            }
            return View(list);
        }
        public ActionResult Presenttrentals()
        {
            List<CarRents> ls = fs.rentlist();
            Customers k = JsonConvert.DeserializeObject<Customers>(TempData["user"].ToString());

            int id = k.Customerid;
            TempData["user"] = JsonConvert.SerializeObject(k);
            ls = ls.Where(x => (x.ReturnDate >= DateTime.Today && x.Customerid == id && x.ReturnOdoReading == null)).ToList();
            List<CARRENT> list = new List<CARRENT>();
            foreach (var rent in ls)
            {
                CARRENT r = new CARRENT();
                r.RentId = rent.RentId;
                r.CarId = rent.Carid;
                r.CustomerId = rent.Customerid;
                r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
                r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
                r.OdoReading = rent.OdoReading;
                r.ReturnOdoReading = rent.ReturnOdoReading;
                r.LicenseNumber = rent.Licensenumber;




                list.Add(r);
            }
            return View(list);
        }

        // GET: Customer/Delete/5
        public ActionResult Cancel(int id)
        {
            CarRents rent = fs.find(id);

            CARRENT r = new CARRENT();
            r.RentId = rent.RentId;
            r.CarId = rent.Carid;
            Cars k2 = cdal.find((int)rent.Carid);
            ViewBag.image = k2.Photo;
            r.CustomerId = rent.Customerid;
            r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
            r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
            r.OdoReading = rent.OdoReading;
            r.ReturnOdoReading = rent.ReturnOdoReading;
            r.LicenseNumber = rent.Licensenumber;

            return View(r);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Cancel(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                fs.Cancel(id);
                return RedirectToAction("Presenttrentals");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Return(int id)
        {
            CarRents rent = fs.find(id);
            CARRENT r = new CARRENT();
            r.RentId = rent.RentId;
            r.CarId = rent.Carid;
            Cars k2 = cdal.find((int)rent.Carid);
            ViewBag.image = k2.Photo;
            r.CustomerId = rent.Customerid;
            r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
            r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
            r.OdoReading = rent.OdoReading;
            r.ReturnOdoReading = rent.ReturnOdoReading;
            r.LicenseNumber = rent.Licensenumber;
            return View(r);
        }
        [HttpPost]
        public ActionResult Return(int id, CARRENT rent)
        {
            try
            {
                CarRents r = new CarRents();
                r.RentId = rent.RentId;
                r.Carid = rent.CarId;
                r.Customerid = rent.CustomerId;
                r.RentOrderDate = Convert.ToDateTime(rent.RentOrderDate);
                r.ReturnDate = Convert.ToDateTime(rent.ReturnDate);
                r.OdoReading = rent.OdoReading;
                r.ReturnOdoReading = rent.ReturnOdoReading;
                r.Licensenumber = rent.LicenseNumber;
                fs.Return(id, r);
                return RedirectToAction("Payment", new { id = r.RentId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Payment(int id)
        {
            CarRents r = fs.find(id);
            Cost k1 = new Cost();
            k1.Rentid = id;
            Tuple<int, double> k = fs.charges(r);
            k1.KmsCovered = k.Item1;
            int id1 = Convert.ToInt32(r.Customerid);
            cd.Addloyalty(k1.KmsCovered, id1);

            k1.Price = k.Item2;
            double charge = k.Item2;
            double tax = 0;
            if (charge < 1000)
            {
                tax = charge * 0.03;
                charge = charge + tax;
            }
            else if (charge < 5000)
            {
                tax = charge * 0.05;
                charge = charge + tax;
            }
            else
            {
                tax = charge * 0.08;
                charge = charge + tax;
            }
            k1.tax = tax;
            k1.TotalCost = charge;
            TempData["Cos"] = JsonConvert.SerializeObject(k1);
            return View(k1);
        }
        public ActionResult ApplyDiscount10(int id)
        {
            Cost k = JsonConvert.DeserializeObject<Cost>(TempData["Cos"].ToString());
            CarRents m = fs.find(id);
            int id1 = Convert.ToInt32(m.Customerid);
            Customers c = cd.GetCustomer(id1);
            if (c.LoyaltyPoints >= 25)
            {
                k.TotalCost = k.TotalCost - (0.1 * k.TotalCost);
                cd.minusloyalty(id1);
                ViewBag.Message5 = "Discount Applied..";
                return View(k);
            }
            else
            {
                ViewBag.Message4 = "Your loyalty Points still not reach the level to get discount";
                return View(k);
            }


        }

        public ActionResult Successful()
        {
            return View();
        }


        public ActionResult preventBack()
        {
            Customers k = JsonConvert.DeserializeObject<Customers>(TempData["user"].ToString());
            string p = k.CustomerName;
            ViewData["status"] = p;
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
