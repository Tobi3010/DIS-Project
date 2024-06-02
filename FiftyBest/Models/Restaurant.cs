namespace Setoma.CompSci.Dis.FiftyBest.Models;

 public class Restaurant
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public string Rank { get; set; }
        public string City { get; set; }
        public Restaurant(string year, string rank, string name, string city){
            Year = year;
            Rank = rank;
            Name = name;
            City = city;
        }
    }