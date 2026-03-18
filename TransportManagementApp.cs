using System;
using System.Linq;
using TransportManagement.Data;
using TransportManagement.Models;

namespace TransportManagement
{
    public class TransportManagementApp
    {
        private readonly TransportDbContext _context;

        public TransportManagementApp(TransportDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            int choice = 0;

            while (choice != 13)
            {
                Console.WriteLine("\n---- Transport Management Menu ----");
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Update Vehicle");
                Console.WriteLine("3. Delete Vehicle");
                Console.WriteLine("4. Schedule Trip");
                Console.WriteLine("5. Cancel Trip");
                Console.WriteLine("6. Book Trip");
                Console.WriteLine("7. Cancel Booking");
                Console.WriteLine("8. Allocate Driver");
                Console.WriteLine("9. Deallocate Driver");
                Console.WriteLine("10. Get Bookings By Passenger");
                Console.WriteLine("11. Get Bookings By Trip");
                Console.WriteLine("12. Get Available Drivers");
                Console.WriteLine("13. Exit");

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Model: ");
                        string model = Console.ReadLine();

                        Console.Write("Enter Capacity: ");
                        decimal capacity = Convert.ToDecimal(Console.ReadLine());

                        Console.Write("Enter Type: ");
                        string type = Console.ReadLine();

                        Console.Write("Enter Status: ");
                        string status = Console.ReadLine();

                        var vehicle = new Vehicle
                        {
                            Model = model,
                            Capacity = capacity,
                            Type = type,
                            Status = status
                        };

                        _context.Vehicles.Add(vehicle);
                        _context.SaveChanges();

                        Console.WriteLine("Vehicle Added Successfully");
                        break;

                        _context.Vehicles.Add(vehicle);
                        _context.SaveChanges();

                        Console.WriteLine("Vehicle Added Successfully");
                        break;

                    case 2:
                        Console.Write("Enter Vehicle ID: ");
                        int vid = Convert.ToInt32(Console.ReadLine());

                        var v = _context.Vehicles.Find(vid);

                        if (v != null)
                        {
                            Console.Write("Enter New Model: ");
                            v.Model = Console.ReadLine();

                            Console.Write("Enter New Capacity: ");
                            v.Capacity = Convert.ToDecimal(Console.ReadLine());

                            Console.Write("Enter New Type: ");
                            v.Type = Console.ReadLine();

                            Console.Write("Enter New Status: ");
                            v.Status = Console.ReadLine();

                            _context.SaveChanges();
                            Console.WriteLine("Vehicle Updated");
                        }
                        else
                        {
                            Console.WriteLine("Vehicle Not Found");
                        }
                        break;

                    case 3:
                        Console.Write("Enter Vehicle ID: ");
                        vid = Convert.ToInt32(Console.ReadLine());

                        v = _context.Vehicles.Find(vid);

                        if (v != null)
                        {
                            _context.Vehicles.Remove(v);
                            _context.SaveChanges();
                            Console.WriteLine("Vehicle Deleted");
                        }
                        else
                        {
                            Console.WriteLine("Vehicle Not Found");
                        }
                        break;

                    case 4:
                        Console.Write("Enter Vehicle ID: ");
                        int vehicleId = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter Route ID: ");
                        int routeId = Convert.ToInt32(Console.ReadLine());

                        var trip = new Trip
                        {
                            VehicleID = vehicleId,
                            RouteID = routeId,
                            DepartureDate = DateTime.Now,
                            ArrivalDate = DateTime.Now.AddHours(2),
                            Status = "Scheduled"
                        };

                        _context.Trips.Add(trip);
                        _context.SaveChanges();

                        Console.WriteLine("Trip Scheduled");
                        break;

                    case 5:
                        Console.Write("Enter Trip ID: ");
                        int tripId = Convert.ToInt32(Console.ReadLine());

                        var t = _context.Trips.Find(tripId);

                        if (t != null)
                        {
                            t.Status = "Cancelled";
                            _context.SaveChanges();
                            Console.WriteLine("Trip Cancelled");
                        }
                        else
                        {
                            Console.WriteLine("Trip Not Found");
                        }
                        break;

                    case 6:
                        Console.Write("Enter Trip ID: ");
                        tripId = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter Passenger ID: ");
                        int pid = Convert.ToInt32(Console.ReadLine());

                        var booking = new Booking
                        {
                            TripID = tripId,
                            PassengerID = pid,
                            BookingDate = DateTime.Now,
                            Status = "Confirmed"
                        };

                        _context.Bookings.Add(booking);
                        _context.SaveChanges();

                        Console.WriteLine("Booking Confirmed");
                        break;

                    case 7:
                        Console.Write("Enter Booking ID: ");
                        int bid = Convert.ToInt32(Console.ReadLine());

                        var b = _context.Bookings.Find(bid);

                        if (b != null)
                        {
                            b.Status = "Cancelled";
                            _context.SaveChanges();
                            Console.WriteLine("Booking Cancelled");
                        }
                        else
                        {
                            Console.WriteLine("Booking Not Found");
                        }
                        break;

                    case 10:
                        Console.Write("Enter Passenger ID: ");
                        pid = Convert.ToInt32(Console.ReadLine());

                        var bookings = _context.Bookings
                            .Where(x => x.PassengerID == pid)
                            .ToList();

                        if (bookings.Count == 0)
                            Console.WriteLine("No Bookings Found");
                        else
                            bookings.ForEach(x => Console.WriteLine($"Booking ID: {x.BookingID}"));

                        break;

                    case 11:
                        Console.Write("Enter Trip ID: ");
                        tripId = Convert.ToInt32(Console.ReadLine());

                        bookings = _context.Bookings
                            .Where(x => x.TripID == tripId)
                            .ToList();

                        if (bookings.Count == 0)
                            Console.WriteLine("No Bookings Found");
                        else
                            bookings.ForEach(x => Console.WriteLine($"Booking ID: {x.BookingID}"));

                        break;
                }
            }
        }
    }
}