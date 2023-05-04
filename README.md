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

