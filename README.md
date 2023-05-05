# Comparison Service

This server is a Comparison Service that access the anonymization python service in this same repository to return embeddings for an image that is received via GRPC by this service.
Another function is to compare two images via cosine similarity and return if it is the same person or not.

---

## Configuration

To run this service you will need to create a database. A docker-compose file is availabe so you can create a postgresdb exactly the way de app needs. To do this run:
```shell
docker-compose up -d
```
To know more about docker and containers access the documentation: https://www.docker.com/

---

To run the app, install dotnet (https://dotnet.microsoft.com/en-us/download) and build the solution (sln):
```shell
dotnet build ./ComparisonService.sln
```
Then, access the binary folder that was generated and search for the Service DLL, path normally goes like "ComparisonService/ComparisonService/bin/Debug/net7.0/".

Finally, to run it:
```shell
./ComparisonService
```
or
```shell
dotnet ./ComparisonService.dll
```

## Branches
There is one version of this service that uses Rest instead of GRPC (The main branch uses GRPC) but it was stopped in mid of development.
The branch name is "anonymizer_rest".

## Variables
The app uses enviroment variables to set a few things, so it is important to get them the right way.
<br>
<br>
**ASPNETCORE_ENVIROMENT** defines if the app run in dev or prod mode, for this first version only dev is configurated, feel free to complete as you want.
<br>
<br>
**ASPNETCORE_URLS** defines the full URL that the ComparisonService will run, host and port, be certain to not use the same port as the other services (AnonymizationService or DB).
<br>
<br>
Some of the variables are setted in the appsettings file, such as the postgres connection string and the URL that the class anonymizer service will use to connect with the python rest api.
