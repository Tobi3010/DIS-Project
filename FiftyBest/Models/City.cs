namespace Setoma.CompSci.Dis.FiftyBest.Models;
public class City
{
    public string Name { get; set; }
    public string Country { get; set; }
    public City(string name, string country){
        Country = country;
        Name = name;
        
    }
}