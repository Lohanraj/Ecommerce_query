using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCourierApp.Models;

namespace SmartCourierApp.Invoices
{
    public interface IInvoiceGenerator
    {
        void GenerateInvoice(CourierBooking booking, double charge);
    }
}
