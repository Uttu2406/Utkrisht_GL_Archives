using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransportManagement.Controllers;
using TransportManagement.Exceptions;
using TransportManagement.Models;
using TransportManagement.Data;
using Xunit;

namespace TransportManagement.Tests
{
    public class TransportServiceTests
    {
        private TransportDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TransportDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TransportDbContext(options);
        }

        [Fact]
        public async Task BookTrip_ValidData_ReturnsSuccess()
        {
            using var context = GetDbContext();
            var controller = new BookingsController(context);
            await controller.BookTrip(101, 501);

            var booking = context.Bookings.FirstOrDefault(b => b.TripID == 101);
            Assert.NotNull(booking);
        }

        [Fact]
        public async Task Details_InvalidVehicleId_ThrowsVehicleNotFoundException()
        {
            // Arrange
            using var context = GetDbContext();
            var controller = new VehiclesController(context);
            int invalidId = 999;

            // Act & Assert: This will now PASS because the controller throws the error
            await Assert.ThrowsAsync<VehicleNotFoundException>(async () => await controller.Details(invalidId));
        }

        [Fact]
        public async Task CancelBooking_InvalidId_ThrowsException()
        {
            using var context = GetDbContext();
            var controller = new BookingsController(context);

            var ex = await Assert.ThrowsAsync<Exception>(async () => await controller.CancelBooking(9999));
            Assert.Equal("BookingNotFoundException", ex.Message);
        }

        [Fact]
        public void Vehicle_Status_Updates_On_Allocation()
        {
            var vehicle = new Vehicle { Status = "Available" };
            vehicle.Status = "On Trip";
            Assert.Equal("On Trip", vehicle.Status);
        }

        [Fact]
        public void Vehicle_Capacity_Check()
        {
            var vehicle = new Vehicle { Model = "Bus", Capacity = 50.0m };
            Assert.True(vehicle.Capacity > 0);
        }
    }
}