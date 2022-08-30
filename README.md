# signup-example
Prototype web application to add users to a database 

## Running the web application

Run both the web and api solutions via Visual Studio, on localhost

It will not work until you setup the DB (see following section)

## DB Setup
- Must be using Postgres 9.1 and newer
- Database initialisation scripts can be found at api/SQL/initialisation_script.sql
- **Create file db_password.txt at api/SQL, containing your DB password (and nothing else)**
- Feel free to edit connection string to fit your preferences (i.e. different username, port number, etc.)

## Testing

### Unit Testing

- Run via 'Test Explorer' in Visual Studio
- Must have api server running whilst running tests
- In ideal situation, tests would use a different database table - but my tests just use the main DB table

### General Testing

I've conducted my own testing, which primarily duplicates most of the unit tests, e.g.

- Ensuring the same email can't be used twice
- Ensuring the form validation works appropriately and notifies the user as such
- Ensuring the 'Confirm Password' field works as expected
- etc.

I've also used my DB access to ensure no plaintext passwords are stored, and all stored data is well-formed

## Security

No plain-text passwords are sent to the server, or stored in the database. The server currently sits in localhost on http, but in reality, would be accessed by https (but again, no passwords in plaintext are sent anyway)

Passwords are salted and hashed, and come to combined character length of 44 characters. Salt is randomised for each password, and stored against the user's entry in the database