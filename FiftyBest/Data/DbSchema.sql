CREATE TABLE Users (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    userName TEXT UNIQUE NOT NULL
);

CREATE TABLE Restaurants (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    restaurantName TEXT NOT NULL,
    cityName TEXT NOT NULL
);

CREATE TABLE Cities (
    cityName TEXT NOT NULL,
    countryName TEXT NOT NULL
);

CREATE TABLE Countries (
    countryName TEXT NOT NULL
);

CREATE TABLE Ranks (
    year TEXT NOT NULL,
    rank TEXT NOT NULL,
    restaurantName TEXT NOT NULL,
    cityName TEXT NOT NULL
);