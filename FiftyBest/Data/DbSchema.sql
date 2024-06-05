CREATE TABLE Users (
    id SERIAL PRIMARY KEY NOT NULL,
    userName TEXT UNIQUE NOT NULL
);

CREATE TABLE Restaurants (
    year INT NOT NULL,
    rank INT NOT NULL,
    restaurantName TEXT NOT NULL,
    cityName TEXT NOT NULL
);

CREATE TABLE Cities (
    cityName TEXT UNIQUE NOT NULL,
    countryName TEXT NOT NULL
);

CREATE TABLE Countries (
    countryName TEXT UNIQUE NOT NULL
);


