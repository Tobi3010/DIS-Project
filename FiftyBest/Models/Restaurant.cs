namespace Setoma.CompSci.Dis.FiftyBest.Models;

 public class Restaurant
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int Rank { get; set; }
        public string City { get; set; }
        public Restaurant(int year, int rank, string name, string city){
            Year = year;
            Rank = rank;
            Name = name;
            City = city;
        }
    }