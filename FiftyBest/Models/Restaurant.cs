namespace Setoma.CompSci.Dis.FiftyBest.Models;

 public class Restaurant
    {
        public string Name { get; }
        public int Year { get; }
        public int Rank { get; }
        public string City { get; }
        public Restaurant(int year, int rank, string name, string city){
            Year = year;
            Rank = rank;
            Name = name;
            City = city;
        }
    }