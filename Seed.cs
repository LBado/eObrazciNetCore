using eObrazci.Data;
using eObrazci.Models;

namespace eObrazci
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            //če je db prazen naredi
            if (!dataContext.Obrazci.Any())
            {
                var obrazci = new List<Obrazec>()
                {
                    new Obrazec(){

                        Student = new Student()
                        {
                            Ime = "Janez",
                            Priimek = "Novak",
                            Spol = "Moski",
                            DatumRojstva = new DateTime(2000, 1, 1),
                            Naslov = new Naslov()
                            {
                                Ulica = "Nova Ulica",
                                HisnaStevilka = "10",
                                PostnaStevilka = 1000,
                                Kraj = "Ljubljana",
                                Drzava = "Slovenia"
                            },
                            Izpit = new List<Izpit>()
                            {
                                new Izpit() {
                                    Naziv = "Geografija",
                                    DatumOprIzpita = new DateTime(2022,4,3),
                                    Ocena = 9
                                },
                                new Izpit() {
                                    Naziv = "Biologija",
                                    DatumOprIzpita = new DateTime(2022,12,1),
                                    Ocena = 10
                                }
                            }
                        }
                    },
                    new Obrazec(){

                        Student = new Student()
                        {
                            Ime = "Mary",
                            Priimek = "Jane",
                            Spol = "Ženska",
                            DatumRojstva = new DateTime(1990, 1, 1),
                            Naslov = new Naslov()
                            {
                                Ulica = "Velika Ulica",
                                HisnaStevilka = "6b",
                                PostnaStevilka = 2000,
                                Kraj = "Maribor",
                                Drzava = "Slovenia"
                            },
                            Izpit = new List<Izpit>()
                            {
                                new Izpit() {
                                    Naziv = "Fizika",
                                    DatumOprIzpita = new DateTime(2022,3,3),
                                    Ocena = 8
                                },
                                new Izpit() {
                                    Naziv = "Zgodovina",
                                    DatumOprIzpita = new DateTime(2022,6,6),
                                    Ocena = 9
                                }
                            }
                        }
                    }
                };
                dataContext.Obrazci.AddRange(obrazci);
                dataContext.SaveChanges();
            }

            if (!dataContext.Users.Any())
            {
                var user = new User()
                {
                    Username = "user",
                    Password = "user"
                };

                dataContext.Users.AddRange(user);
                dataContext.SaveChanges();
            }
        }
    }
}
