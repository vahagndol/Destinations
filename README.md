# Destinations Demo Application
[![Build Status](https://travis-ci.com/vahagndol/Destinations.svg?branch=master)](https://travis-ci.com/vahagndol/Destinations)

## Introduction
This is a demonstartion of a destination web application using microservices.  
The application interacts with microservices using HTTP requests.  


## What's included
- **Application** - A client that makes requests to the microservices(License.API a Places.API) and shows infromation about destinations.
- **Domain** - Contains all the domain entities.
- **Infrastructure** - The infrastructure layer of Onion Architecture. Deals with repositories.
- **Locations.API** - The microservice for Location API calls.
- **Places.API** - The microservice for Places API calls.

## Running the demonstration application
- Clone this repository.
- Build projects: 
> *dotnet build src\Locations.API
> *dotnet build src\Places.API
> *dotnet build src\Application
- Run projects:
> *dotnet run --project src\Locations.API\Locations.API.csproj
> *dotnet run --project src\Places.API\Places.API.csproj
> *dotnet run --project src\Application\Application.csproj
- Navigate to the URLs listed below.

Another option:
- Loading the project in Visual Studio.
- Start the debugger with docker-compose.
- This should load the Basket API swagger page.

3 containers should be created:
- **basketapi** - http://localhost:55311/swagger/index.html
- **clientapp** - http://localhost:55322/swagger/index.html
- **rabbit** - http://localhost:15672/#/
  - Management page for monitoring.
  - Username: guest
  - Password: guest

This solution was developed on Windows 10, using:
- Visual Studio 15.7.4
- .NET Core SDK 2.1.301
- Docker 18.03.1 using Linux containers.
- RabbitMQ 3.7.6 from Docker.

![Diagram](Diagram2.png)

## Using the API
- The swagger page for the Basket API shows all of the actions available to users of the API.
- The swagger page for the ClientApp shows how a potential user could use the API.
- The Client App can simulate another service changing the price of a product. This publishes a price change event to a message broker.  The Basket API is subscribed to this broker and will update the price of this product in any basket it happens to be in.
- The Client App also demonstrates the use of the circuit breaker pattern.  This can be seen by disabling the Basket API container and making a few requests from the client app.  Once the Basket API is turned back on, requests will resume after 1 minute.  
