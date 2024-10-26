# Vehicle Rental API

This is the Vehicle Rental API project. It has
* GET endpoint for availability of _vehicle types_ given a pickup and return date and an optional set of vehicle types (if none are provided, then all types are returned)
* POST endpoint for making a reservation for a pickup and return dates and a single required vehicle type

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional [Docker](https://docs.docker.com/engine/install/)

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

### Running Tests

To run the tests, use the following command:
```sh
dotnet test
```

### Running the Application

To run the application, use the following command:
```sh
dotnet run
```

The API will be available at `http://localhost:8080/swagger/index.html`.


## Assumptions

* Bookings must span at least one full day, meaning same-day pickup and return are not permitted.

* Vehicles returned on a given day are available for new bookings starting on that same day.

## TODOs

- **Foreign Key Relations**: FK relations and carefully indexed db.
- **Datetime Handling**: Datetime handling needs to be culture invariant.
- **Exception Handling**: Generic 5xx exceptions should be handled better by responding with user-friendly messages and appropriate status codes.
- **Logging**: Logging is not implemented at all.
- **Integration Tests**: Integration tests need to be added.