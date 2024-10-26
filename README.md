# Vehicle Rental API

This is the Vehicle Rental API project. It has
* GET endpoint for availability of _vehicle types_ given a pickup and return date and an optional set of vehicle types (if none are provided, then all types are returned)
* POST endpoint for making a reservation for a pickup and return dates and a single required vehicle type

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/singhrahulnet/car-rental-api.git
    cd car-rental-api/VehicleRentalApi
    ```

2. Restore the dependencies:
    ```sh
    dotnet restore
    ```

### Running the Application

To run the application, use the following command:
```sh
dotnet run
```

The API will be available at `http://localhost:8080/swagger/index.html`.

### Running Tests

To run the tests, use the following command:
```sh
dotnet test
```

## TODOs

- **Foreign Key Relations**: FK relations and carefully indexed db.
- **Datetime Handling**: Datetime handling needs to be culture invariant.
- **Integration Tests**: Integration tests need to be added.