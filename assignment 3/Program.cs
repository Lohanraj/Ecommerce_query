using SmartCourierApp.DeliveryCalculators;
using SmartCourierApp.Invoices;

using SmartCourierApp.Models;
using SmartCourierApp.Notifications;
using SmartCourierApp.Services;
using System;

namespace SmartCourierApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();

            Console.Write("Enter Customer Name : ");
            customer.Name = Console.ReadLine();

            Console.Write("Enter Email : ");
            customer.Email = Console.ReadLine();

            Console.Write("Enter Mobile Number : ");
            customer.MobileNumber = Console.ReadLine();

            Parcel parcel = new Parcel();

            Console.Write("Enter Parcel Weight : ");
            parcel.Weight = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Source City : ");
            parcel.SourceCity = Console.ReadLine();

            Console.Write("Enter Destination City : ");
            parcel.DestinationCity = Console.ReadLine();

            Console.WriteLine("\nDelivery Types");
            Console.WriteLine("1. Standard");
            Console.WriteLine("2. Express");
            Console.WriteLine("3. International");

            Console.Write("Choose Delivery Type : ");
            int deliveryChoice = Convert.ToInt32(Console.ReadLine());

            IDeliveryChargeCalculator calculator = null;
            string deliveryType = "";

            switch (deliveryChoice)
            {
                case 1:
                    calculator = new StandardDeliveryCalculator();
                    deliveryType = "Standard";
                    break;

                case 2:
                    calculator = new ExpressDeliveryCalculator();
                    deliveryType = "Express";
                    break;

                case 3:
                    calculator = new InternationalDeliveryCalculator();
                    deliveryType = "International";
                    break;
            }

            Console.WriteLine("\nNotification Types");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            Console.WriteLine("3. WhatsApp");

            Console.Write("Choose Notification Type : ");
            int notificationChoice =
                Convert.ToInt32(Console.ReadLine());

            INotificationService notificationService = null;

            switch (notificationChoice)
            {
                case 1:
                    notificationService =
                        new EmailNotificationService();
                    break;

                case 2:
                    notificationService =
                        new SmsNotificationService();
                    break;

                case 3:
                    notificationService =
                        new WhatsAppNotificationService();
                    break;
            }

            CourierBooking booking = new CourierBooking
            {
                Customer = customer,
                Parcel = parcel,
                DeliveryType = deliveryType
            };

            IInvoiceGenerator invoiceGenerator =
                new ConsoleInvoiceGenerator();

            CourierBookingService bookingService =
                new CourierBookingService(
                    calculator,
                    notificationService,
                    invoiceGenerator);

            bookingService.BookCourier(booking);

            Console.ReadKey();
        }
    }
}