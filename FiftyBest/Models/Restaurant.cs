namespace Setoma.CompSci.Dis.FiftyBest.Models;

 public class Restaurant
    {
        public string Name { get; }
        public string Year { get; }
        public string Rank { get; }
        public string City { get; }
        public Restaurant(int year, int rank, string name, string city){
            Year = year.ToString();
            Rank = rank.ToString();
            Name = name;
            City = city;
        }
    }