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
> dotnet build src\Locations.API
> dotnet build src\Places.API
> dotnet build src\Application
- Run projects:
> dotnet run --project src\Locations.API\Locations.API.csproj
> dotnet run --project src\Places.API\Places.API.csproj
> dotnet run --project src\Application\Application.csproj
- Navigate to the URLs listed below.

Another option:
- Loading the project in Visual Studio.
- Build solution
- Run Locations.API.
- Run Places.API.
- Run Application.

3 console should be created:
- **Locations.API** - http://localhost:5001/swagger/index.html
- **Places.API** - http://localhost:4001/swagger/index.html
- **Application** - http://localhost:56986/

This solution was developed on Windows 10, using:
- Visual Studio 15.7.4
- .NET Core SDK 2.1.300

## Azure
The latest versions of Applications and API Services deployed to Azure Cloud:
- **Locations.API** - https://destinationslocationsapi.azurewebsites.net/swagger/index.html
- **Places.API** - https://destinationsplacesapi.azurewebsites.net/swagger/index.html
- **Application** - https://destinationswebapp.azurewebsites.net/