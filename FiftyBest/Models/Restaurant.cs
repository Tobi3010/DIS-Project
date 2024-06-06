namespace Setoma.CompSci.Dis.FiftyBest.Models;

public sealed record Restaurant(
    int Id,
    int Year,
    int Rank,
    string Name,
    string City);