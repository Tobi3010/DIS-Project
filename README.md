# Fifty Best

This application contains historical data from the World's 50 Best restaurants lists.

## Setup
This section contains installation and setup instructions.

### Database setup
The database schema is defined in `.sql` files in the `Data` directory, but the easiest way to set up the database is to execute the setup script:

```
$ ./create-db.sh
```

This script will prompt you (thrice) to enter your password for your PostgreSQL installation.

If you want to delete the database again, you can use the script:

```
$ drop-db.sh
```

This script will also require your password.

### Application configuration
The application is an ASP.NET application. Before running it, you must edit the `appsettings.json` file by adding a connection string to the database you just created.

The configuration entry should have the format:

```
"Restaurants": "Host=<host-name>;Username=<un>;Password=<pw>;Database=fifty-best"
```

If your PostgreSQL installation is on your local machine, you should be able to use either `localhost` or `127.0.0.1` as the `host-name`.

### Running the application
You should now be able to run the application:

```
$ ./run.sh
```

If all goes well, this script should start up the application and tell you the address on which it's running. Example:

```
$ ./run.sh
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5039
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: <redacted>
```

Notice that the `Now listening on` address may be different on your system. 


## Userguide

Now that it is up and running, heres a quick userguide for our webapp :

### User page
This is a good place to start. 

- You can create a new user, or login to an already existing user (users
are only usernames, no password). Then you can also log off.

- If you want, you can change you username to something else. By typing a username 
Into **New username**, and click **Change**.

- Once you have marked some restaurants as 
  - [x] Visited 

  on the restaurant page, you will be able to see those restaurants on this page,
  And the score you gave.


### Home page
On the home page, you can see the top 50 restaurants from 2023.

- If you wanna see the 50 best restaurants from other years, use the 
**Enter Year between 2002 and 2023**, which accepts the following inputs;

    - Year     : years from 2002 to 2023, exept 2020
    - List     : `year1,year2,year3`
    - Interval : `year1-year2` where year2 => year1
 
- If you only wanna see restaurants from a certain country, use the 
**Select country** dropdown menu, pick a country and click **Filter**.

- Once a country is selected, you can now pick a city the same way
you picked a country.

- You can click on the blue link of each restaurant, to go to that 
Restaurant's page.


### Restaurant page
On the restaurant page, you can see all the times that restaurant appeared
In top 50 list. 

- You can click on the 
   - [ ] Visited 

  checkbox, to mark that restaurant as 
visited. Which can now be seen on your user page.

- Once a restaurant is marked as visited, you can also give it a score.
Scores range from 0 to 5, with increments of 0.5. The score can be seen on
Your userpage.