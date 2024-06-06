namespace Setoma.CompSci.Dis.FiftyBest.Models;

 public class Restaurant
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public string Rank { get; set; }
        public string City { get; set; }
        public Restaurant(int year, int rank, string name, string city){
            Year = year.ToString();
            Rank = rank.ToString();
            Name = name;
            City = city;
        }
    }