using Mechanicshop.Application.Features.Customers.Mappers;
using Mechanicshop.Domain.Customers;
using Mechanicshop.Domain.Customers.Vehicles;
using MechanicShop.Tests.Common.Customers;
using Xunit;

namespace MechanicShop.Application.UnitTests.Mappers;

public class CustomerMapperTest
{
    [Fact]
    public void ToDto_ShouldMapCorrectly()
    {
        var vehicle = VehicleFactory.CreateVehicle(Guid.NewGuid(), "Honda", "Accord", 2024, "ABC 123").Value;

        var customer = CustomerFactory.CreateCustomer(
            Guid.NewGuid(),
            "New Customer",
            "01299866543",
            "validMail123@gmail.com",
            [vehicle]
            ).Value;

        var dto = customer.ToDTO();

        Assert.Equal(customer.Id, dto.CustomerId);
        Assert.Equal(customer.Name, dto.Name);
        Assert.Equal(customer.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(customer.Email, dto.Email);
        Assert.Equal(customer.Vehicles.ToDTOs(), dto.Vehicles);

        Assert.Single(dto.Vehicles);
    }
    [Fact]
    public void ToDtos_ShouldMapCustomerListCorrectly()
    {
        var vehicle = VehicleFactory.CreateVehicle(Guid.NewGuid(), "Toyota", "Camry", 2023, "XYZ 789").Value;

        var customer = CustomerFactory.CreateCustomer(
            Guid.NewGuid(),
            "Another Customer",
            "01122334455",
            "anotherMail@gmail.com",
            [vehicle]
            ).Value;

        var customers = new List<Customer> { customer };

        var dtos = customers.ToDTOs();

        Assert.Single(dtos);
        Assert.Equal(customers[0].Id, dtos[0].CustomerId);
        Assert.Equal(customers[0].Name, dtos[0].Name);
    }

    [Fact]
    public void VehicleToDto_ShouldMapCorrectly()
    {
        var vehicle = VehicleFactory.CreateVehicle(Guid.NewGuid(), "Ford", "Mustang", 2022, "DEF 456").Value;

        var dto = vehicle.ToDTO();

        Assert.Equal(vehicle.Id, dto.VehicleId);
        Assert.Equal(vehicle.Make, dto.Make);
        Assert.Equal(vehicle.Model, dto.Model);
        Assert.Equal(vehicle.Year, dto.Year);
        Assert.Equal(vehicle.LicensePlate, dto.LicensePlate);
    }

    [Fact]
    public void VehicleToDtos_ShouldMapVehicleListCorrectly()
    {
        var vehicle1 = VehicleFactory.CreateVehicle(Guid.NewGuid(), "Honda", "Accord", 2024, "ABC 123").Value;
        var vehicle2 = VehicleFactory.CreateVehicle(Guid.NewGuid(), "Toyota", "Corolla", 2021, "XYZ 999").Value;

        var vehicles = new List<Vehicle> { vehicle1, vehicle2 };

        var dtos = vehicles.ToDTOs();

        Assert.Equal(2, dtos.Count);
        Assert.Equal(vehicles[0].Id, dtos[0].VehicleId);
        Assert.Equal(vehicles[1].Id, dtos[1].VehicleId);
    }

    [Fact]
    public void ToDto_ShouldThrowArgumentNullException_WhenCustomerIsNull()
    {
        Customer? customer = null;

        Assert.Throws<ArgumentNullException>(() => customer!.ToDTO());
    }

    [Fact]
    public void VehicleToDto_ShouldThrowArgumentNullException_WhenVehicleIsNull()
    {
        Vehicle? vehicle = null;

        Assert.Throws<ArgumentNullException>(() => vehicle!.ToDTO());
    }
}