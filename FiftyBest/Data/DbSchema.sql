CREATE TABLE Users (
    id SERIAL PRIMARY KEY NOT NULL,
    userName TEXT UNIQUE NOT NULL
);