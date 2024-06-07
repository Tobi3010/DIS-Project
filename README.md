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