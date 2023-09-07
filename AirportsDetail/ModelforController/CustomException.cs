using Newtonsoft.Json;

namespace CarRental.ModelforController
{
    public class CustomException
    {
        public int statuscode
        {
            get;
            set;
        }
        public string message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
