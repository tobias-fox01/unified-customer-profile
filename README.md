### unified-customer-profile

## About The Project

Unified Customer Profile
A middleware platform for aggregating and normalising customer data across services.
The aim of this project is create endpoints that aggregating across multiple data sources that would reflect a real life system or company that will have data fragmented across multiple interal and external rescources.

## Built With

- .Net v10.0.0

## Setup

1. Clone the repository
```sh
git clone git@github.com:tobias-fox01/unified-customer-profile.git
```

2. Install dependancies
```sh
dotnet restore
```

3. Build Project
```sh
dotnet build
```

To setup this project, you must locally create the cosmos databases which will represent mock external systems.
4. Create the container using Podman to run cosmos db emulator
```sh
podman compose --file compose.yml up --detach
```

5. To upload the data from json to cosmos db database run
```sh
dotnet run --project ./unified-customer-profile.Setup
```

6. To run the project itself run, where unified-customer-profile.Api is the main entry point
```sh
dotnet run --project ./unified-customer-profile.Api
```

## Contact
Developers: Tobias Fox