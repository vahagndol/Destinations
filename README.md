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
