# HigherPercentile
A simple web app developed with C# and ASP.NET, it gathers stock data via an API and computes the momentum score for the user. It has general user and watchlist functionality.

ASP.NET Core 6.0 and SQL

To run:
-Make sure to change your DB connection in appsettings.json
-You can migrate the DB with Package Manager Console with the following CMDs 
    1st cmd: add-migration nameOfMigration 
    2nd cmd: update-database

sidenote: each API key has a 100 daily call limit
