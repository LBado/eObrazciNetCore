//using CountryNamesSOAP;
using eObrazci.Data;
using eObrazci.Interfaces;
using eObrazci.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceReference;
using System.Data;
using System.ServiceModel;
using System.Text;

namespace eObrazci.Repository
{
    public class ObrazecRepository : IObrazecRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public readonly string serviceUrl = "http://localhost:8080/ws/countries.wsdl";
        public readonly EndpointAddress endpointAddress;
        public readonly BasicHttpBinding basicHttpBinding;

        public ObrazecRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            
            //vvv---SOAP settings
            endpointAddress = new EndpointAddress(serviceUrl);
            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "http" ?
                             BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
            basicHttpBinding.OpenTimeout = TimeSpan.MaxValue;
            basicHttpBinding.CloseTimeout = TimeSpan.MaxValue;
            basicHttpBinding.ReceiveTimeout = TimeSpan.MaxValue;
            basicHttpBinding.SendTimeout = TimeSpan.MaxValue;
        }

        public async Task<string> GetCountryNameByISOPublicSOAP(string countryISO)
        {
            string country;
            try
            {
                CountriesPortClient client = new CountriesPortClient();
                ServiceReference.getCountryRequest request = new ServiceReference.getCountryRequest();
                request.name = countryISO;

                //var client = await GetInstanceAsync();
                var result = client.getCountry(request);
                country = result.country;

            }
            catch (Exception ex)
            {
                country = "error";
            }
            return country;
        }

        //public async Task<string> GetCountryNameByISOPublicSOAP(string countryISO)
        //{
        //    string country;
        //    //ISO code v SOAP da dobimo country
        //    //CountryNameAsync(string sCountryISOCode)
        //    try
        //    {
        //        var client = await GetInstanceAsync();
        //        var result = await client.CountryNameAsync(countryISO);
        //        country = result.Body.CountryNameResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        country = "error";
        //    }
        //    return country;
        //}

        
        public bool CreateObrazec(Student student)
        {
            //če imamo FK jih moramo najti in vnesti
            var obrazec = new Obrazec()
            {
                Student = student,
            };

            _context.Add(obrazec);

            return Save();
        }

        public bool DeleteObrazec(Obrazec obrazec)
        {
            _context.Remove(obrazec);

            return Save();
        }

        //SOAP implementation
        //public async Task<CountryInfoServiceSoapTypeClient> GetInstanceAsync()
        //{
        //    return await Task.Run(() => new CountryInfoServiceSoapTypeClient(basicHttpBinding, endpointAddress));
        //}

        public async Task<CountriesPortClient> GetInstanceAsync()
        {
            return await Task.Run(() => new CountriesPortClient(basicHttpBinding, endpointAddress));
        }

        public ICollection<Obrazec> GetObrazci()
        {
            return _context.Obrazci
                .Include(s => s.Student)
                    .ThenInclude(std => std.Naslov)
                .Include(s => s.Student)
                    .ThenInclude(std => std.Izpit)
                .OrderBy(o => o.Id)
                .ToList();
        }

        public Obrazec GetObrazec(int id)
        {
            return _context.Obrazci.Where(o => o.Id == id)
                .Include(s => s.Student)
                    .ThenInclude(std => std.Naslov)
                .Include(s => s.Student)
                    .ThenInclude(std => std.Izpit)
                .FirstOrDefault();
        }

        public Obrazec GetObrazecDetail(int id)
        {

            return _context.Obrazci
                .Where(o => o.Id == id)
                .Include(s=>s.Student)
                    .ThenInclude(std => std.Naslov)
                .Include(s=>s.Student)
                    .ThenInclude(std=>std.Izpit)
                .FirstOrDefault();
        }

        public bool ObrazecExists(int id)
        {
            return _context.Obrazci.Any(o => o.Id == id);
        }

        public bool Save()
        {
            //shrani vse v tem contextu v DB, vrne število entitet shranjenih v db
            //converta v sql in pošlje v db
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateObrazec(Obrazec obrazec)
        {
            _context.Update(obrazec);

            return Save();
        }

        
    }
}
