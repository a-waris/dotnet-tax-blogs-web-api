# TaxBox Web Api

A ``.Net 7.0`` WebApi Taxbox / template project. MediatR, Swagger, Mapster, Serilog and more implemented. 

# How to run
- Use this template(github) or clone/download to your local workplace.
- Download the latest .Net SDK and Visual Studio/Code.

## Standalone
1. You may need a running instance of MsSQL, with appropriate migrations initialized.
	- You can run just the DB on docker. For that, you have to change your connection string to "Server=127.0.0.1;Database=master;User=sa;Password=Yourpassword123” and run the following command: ``docker-compose up -d db-server``. Doing that, the application will be able to reach the container of the db server.
	- If you want, you can change the DatabaseExtension to use UseInMemoryDatabase, instead of Mssql.
2. Go to the src/Taxbox.Api folder and run ``dotnet run``, or, in visual studio set the api project as startup and run as console or docker (not IIS).
3. Visit http://localhost:5000/api-docs or https://localhost:5001/api-docs to access the application's swagger.

## Docker
1. Run ``docker-compose up -d`` in the root directory, or, in visual studio, set the docker-compose project as startup and run. This should start the application and DB.
 - 1. For docker-compose, you should run this command on the root folder: ``dotnet dev-certs https -ep https/aspnetapp.pfx -p yourpassword``
		Replace "yourpassword" with something else in this command and the docker-compose.override.yml file.
This creates the https certificate.
2. Visit http://localhost:5000/api-docs or https://localhost:5001/api-docs to access the application's swagger.

## Running tests
**Important**: You need to have docker up and running. The integration tests will launch a SQL server container and use it to test the API.

In the root folder, run ``dotnet test``. This command will try to find all test projects associated with the sln file.
If you are using Visual Studio, you can also access the Test Menu and open the Test Explorer, where you can see all tests and run all of them or one specifically. 

## Authentication
In this project, some routes requires authentication/authorization. For that, you will have to use the ``api/user/authenticate`` route to obtain the JWT.
As default, you have two users, Admin and normal user.
- Normal user: 
	- email: user@taxbox.com
	- password: userpassword
- Admin:
	- email: admin@taxbox.com
	- password: adminpassword

After that, you can pass the jwt on the lock (if using swagger) or via the Authorization header on a http request.

# This project contains:
- SwaggerUI
- EntityFramework
- Strongly Typed Ids
- Mapster
- MediatR
- Feature slicing
- Serilog with request logging and easily configurable sinks
- .Net Dependency Injection
- Resource filtering
- Response compression
- Response pagination
- CI (Github Actions)
- Authentication
- Authorization
- Unit tests
- Integration tests
- Container support with [docker](src/Taxbox.Api/dockerfile) and [docker-compose](docker-compose.yml)
- OpenTelemetry support (with jaeger as default exporter)
- NuGet Central package management (CPM)

# Project Structure
1. Services
	- This folder stores your apis and any project that sends data to your users.
	1. Taxbox.Api
		- This is the main api project. Here are all the controllers and initialization for the api that will be used.
	2. docker-compose
		- This project exists to allow you to run docker-compose with Visual Studio. It contains a reference to the docker-compose file and will build all the projects dependencies and run it.
2. Application
	-  This folder stores all data transformations between your api and your domain layer. It also contains your business logic.
	1. Auth
		- This folder contains the login Session implementation.
3. Domain
	- This folder contains your business models, enums and common interfaces.
	1. Taxbox.Domain
		- Contains business models and enums.
		1. Auth
			- This folder contains the login Session Interface.
4. Infra
	- This folder contains all data access configuration, database contexts, anything that reaches for outside data.
	1. Taxbox.Infrastructure
		- This project contains the dbcontext, entities configuration and migrations.


# Migrations
1. To run migrations on this project, run the following command on the root folder: 
	- ``dotnet ef migrations add InitialCreate --startup-project .\src\Taxbox.Api\ --project .\src\Taxbox.Infrastructure\``

2. This command will set the entrypoint for the migration (the responsible to selecting the dbprovider { sqlserver, mysql, etc } and the connection string) and the selected project will be "Taxbox.Infrastructure", which is where the dbcontext is.

# About
[MIT license](LICENSE).
