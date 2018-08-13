![Build badge](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/788/badge)

# Employer Recruit (Private Beta)

This respository represents the Employer Recruit code base currently in beta.

## Developer setup

### Requirements

In order to run this solution locally you will need the following:

* An employer account setup against the DAS test environment
* [.NET Core SDK >= 2.1.4](https://www.microsoft.com/net/download/)
* (VS Code Only) [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
* [Docker for X](https://docs.docker.com/install/#supported-platforms)
* Optionally run [Azure Cosmos DB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) locally instead of MongoDB docker container (Windows Only)

### Environment Setup

The default development environment uses docker containers to host it's dependencies.

* Redis
* Elasticsearch
* Logstash
* MongoDb
* Azurite (Cross platform Azure Storage Emulator)

On first setup run the following command from _**/setup/containers/**_ to create the docker container images:

`docker-compose build`

To start the containers run:

`docker-compose up -d`

You can view the state of the running containers using:

`docker ps -a`


### Running

* Open command prompt and change directory to _**/src/Jobs/Recruit.Vacancies.Jobs/**_
* The azure webjobs require a "real" azure storage account in order to run. Add a valid connection string in **_appSetting.Development.json_** for the following keys:
```
{
  "ConnectionStrings": {
    "WebJobsDashboard": "<replace with connection string to a azure storage account (not local storage emulator)>",
    "WebJobsStorage": "<replace with connection string to a azure storage account (not local storage emulator)>"
  }
}
```
* Start the **Webjobs**:

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
* Open second command prompt and change directory to _**/src/Employer/Employer.Web/**_
* Start the **Website**:

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
* Browse to `http://localhost:5020/accounts/{employerAccountId}`

### Application logs
Application logs are logged to [Elasticsearch](https://www.elastic.co/products/elasticsearch) and can be viewed using [Kibana](https://www.elastic.co/products/kibana) at http://localhost:5601

### Development 

#### Website

* Open solution _**/src/Employer/Employer.sln**_

#### Webjobs

* Open solution _**/src/Jobs/Jobs.sln**_


## License

Licensed under the [MIT license](LICENSE)
