using SmartCourierApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCourierApp.Invoices
{
    public class ConsoleInvoiceGenerator :IInvoiceGenerator
    {
        public void GenerateInvoice(CourierBooking booking, double charge)
        {
            Console.WriteLine("\n===== INVOICE =====");

            Console.WriteLine($"Customer Name : {booking.Customer.Name}");
            Console.WriteLine($"Source City : {booking.Parcel.SourceCity}");
            Console.WriteLine($"Destination City : {booking.Parcel.DestinationCity}");
            Console.WriteLine($"Parcel Weight : {booking.Parcel.Weight} KG");
            Console.WriteLine($"Delivery Type : {booking.DeliveryType}");
            Console.WriteLine($"Total Charge : Rs.{charge}");

            Console.WriteLine("===================");
        }

    }
}
