import os

def csv_to_sql(years):
    sqlLines = []
    countries = []
    cities = []

    for year in years:
        read_path = os.path.join("Data",year+".csv")
        with open(read_path, 'r') as read_file:
            for csvLine in read_file:
                parts = csvLine.replace("'","").strip().split(',')
                
                rank = parts[0]
                restaurantName = parts[1]
                cityName = parts[2]
                countryName = parts[3]

                sqlLines.append(f"INSERT INTO Restaurants (year, rank, restaurantName, cityName) "
                                    f"VALUES ('{year}', '{rank}', '{restaurantName}', '{cityName}');")
                if cityName not in cities:
                    sqlLines.append(f"INSERT INTO Cities (cityName, countryName) "
                                        f"VALUES ('{cityName}', '{countryName}');")
                    cities.append(cityName)
                if countryName not in countries:  
                    sqlLines.append(f"INSERT INTO Countries (countryName) "
                                        f"VALUES ('{countryName}');")
                    countries.append(countryName)

    write_path = os.path.join("Data", "all.sql")
    with open(write_path, 'w') as write_file:
        for sqlLine in sqlLines:
            write_file.write(sqlLine + '\n')

if __name__ == "__main__":
    years = ["2023",
             "2022",
             "2021",
             #2020 not in dataset
             "2019",
             "2018",
             "2017",
             "2016",
             "2015", 
             "2014",
             "2013",
             "2012",
             "2011",
             "2010",
             "2009",
             "2008",
             "2007",
             "2006",
             "2005",
             "2004",
             "2003",
             "2002"]
    csv_to_sql(years)
