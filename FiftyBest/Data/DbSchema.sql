CREATE TABLE Users (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    userName TEXT UNIQUE NOT NULL
);

CREATE TABLE Restaurants (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
    restaurantName TEXT NOT NULL,
    cityName TEXT NOT NULL,
    UNIQUE (restaurantName, cityName)
);

CREATE TABLE Visits (
    score TEXT,
    userId INT REFERENCES Users(id) NOT NULL,
    restaurantId INT REFERENCES Restaurants(id) NOT NULL,
    PRIMARY KEY (userId, restaurantId)
);

CREATE TABLE Cities (
    cityName TEXT NOT NULL,
    countryName TEXT NOT NULL
);

CREATE TABLE Countries (
    countryName TEXT NOT NULL
);

CREATE TABLE Ranks (
    year INT NOT NULL,
    rank INT NOT NULL,
    restaurantId INT REFERENCES Restaurants(id) NOT NULL
);