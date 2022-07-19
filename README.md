# HigherPercentile
A simple web app developed with C# and ASP.NET, it gathers stock data via an API and computes the momentum score for the user. It has general user and watchlist functionality.

ASP.NET Core 6.0 and SQL

To run:
1. Make sure to change your DB connection in appsettings.json
2. You can migrate the DB with the Package Manager Console using the following CMDs -
    1st CMD: add-migration nameOfMigration 
    2nd CMD: update-database

Sidenote: Each API key has a 100 daily call limit. Most of the CSS is from a Bootswatch theme.
