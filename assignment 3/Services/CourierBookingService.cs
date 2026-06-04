using SmartCourierApp.DeliveryCalculators;
using SmartCourierApp.Invoices;
//using SmartCourierApp.Invoices;
using SmartCourierApp.Models;
using SmartCourierApp.Notifications;

namespace SmartCourierApp.Services
{
    public class CourierBookingService
    {
        private readonly IDeliveryChargeCalculator _calculator;
        private readonly INotificationService _notificationService;
        private readonly IInvoiceGenerator _invoiceGenerator;

        public CourierBookingService(
            IDeliveryChargeCalculator calculator,
            INotificationService notificationService,
            IInvoiceGenerator invoiceGenerator)
        {
            _calculator = calculator;
            _notificationService = notificationService;
            _invoiceGenerator = invoiceGenerator;
        }

        public void BookCourier(CourierBooking booking)
        {
            double charge =
                _calculator.CalculateCharge(booking.Parcel.Weight);

            _notificationService.SendNotification(
                "Your courier has been booked successfully.");

            _invoiceGenerator.GenerateInvoice(booking, charge);
        }
    }
}