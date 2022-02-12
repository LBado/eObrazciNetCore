//using CountryNamesSOAP;
using eObrazci.Models;
using ServiceReference;

namespace eObrazci.Interfaces
{
    public interface IObrazecRepository
    {
        ICollection<Obrazec> GetObrazci();
        Obrazec GetObrazec(int id);
        Obrazec GetObrazecDetail(int id);
        bool ObrazecExists(int id);
        bool CreateObrazec(Student student);
        bool UpdateObrazec(Obrazec obrazec);
        bool DeleteObrazec(Obrazec obrazec);
        bool Save();

        Task<CountriesPortClient> GetInstanceAsync();
        Task<string> GetCountryNameByISOPublicSOAP(string countryISO);

    }
}
