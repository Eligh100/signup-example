# signup-example
Prototype web application to add users to a database 

## DB Setup
- Must be using Postgres 9.1 and newer
- Database initialisation scripts can be found at api/SQL/initialisation_script.sql
- Create file db_password.txt at api/SQL, containing your DB password (and nothing else)
- Feel free to edit connection string to fit your preferences (i.e. different username, port number, etc.)

## Testing
- Must have server running whilst running tests
- In ideal situation, tests would use a different database table - but my tests just use the main DB table