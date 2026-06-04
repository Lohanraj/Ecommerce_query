# SmartCourierApp - SOLID Principles Implementation

## Project Overview

SmartCourierApp is a C# .NET Console Application developed for managing courier bookings.

The application allows customers to:

* Book courier deliveries
* Calculate delivery charges
* Receive booking notifications
* Generate invoices

The project is designed by following all five SOLID principles to ensure maintainability, scalability, flexibility, and clean architecture.

---

# S - Single Responsibility Principle (SRP)

## Definition

A class should have only one reason to change and should be responsible for only one functionality.

## Implementation in SmartCourierApp

### Delivery Charge Calculation

Classes:

* StandardDeliveryCalculator
* ExpressDeliveryCalculator
* InternationalDeliveryCalculator

Responsibility:

Calculate delivery charges only.

### Notification Services

Classes:

* EmailNotificationService
* SmsNotificationService
* WhatsAppNotificationService

Responsibility:

Send notifications only.

### Invoice Generation

Class:

* ConsoleInvoiceGenerator

Responsibility:

Generate invoice only.

### Booking Management

Class:

* CourierBookingService

Responsibility:

Manage courier booking process only.

## Benefit

Changes in one functionality do not affect other functionalities.

---

# O - Open Closed Principle (OCP)

## Definition

Software entities should be open for extension but closed for modification.

## Implementation in SmartCourierApp

All delivery calculators implement:

```csharp
IDeliveryChargeCalculator
```

Current implementations:

* StandardDeliveryCalculator
* ExpressDeliveryCalculator
* InternationalDeliveryCalculator

If a new delivery type is introduced, such as Drone Delivery:

```csharp
public class DroneDeliveryCalculator
    : IDeliveryChargeCalculator
{
    public double CalculateCharge(double weight)
    {
        return weight * 120;
    }
}
```

No existing code needs modification.

## Benefit

New functionality can be added without changing tested code.

---

# L - Liskov Substitution Principle (LSP)

## Definition

Derived classes should be replaceable for their base type without affecting program correctness.

## Implementation in SmartCourierApp

All delivery calculators implement:

```csharp
IDeliveryChargeCalculator
```

Example:

```csharp
IDeliveryChargeCalculator calculator;

calculator = new StandardDeliveryCalculator();

calculator = new ExpressDeliveryCalculator();

calculator = new InternationalDeliveryCalculator();
```

Any calculator can replace another without affecting the booking service.

## Benefit

Improves flexibility and interchangeability.

---

# I - Interface Segregation Principle (ISP)

## Definition

Clients should not be forced to depend on interfaces they do not use.

## Implementation in SmartCourierApp

### Notification Interface

```csharp
public interface INotificationService
{
    void SendNotification(string message);
}
```

Implemented by:

* EmailNotificationService
* SmsNotificationService
* WhatsAppNotificationService

### Invoice Interface

```csharp
public interface IInvoiceGenerator
{
    void GenerateInvoice(
        CourierBooking booking,
        double charge);
}
```

Implemented by:

* ConsoleInvoiceGenerator

Notification classes are not forced to generate invoices.

Invoice classes are not forced to send notifications.

## Benefit

Interfaces remain small and focused.

---

# D - Dependency Inversion Principle (DIP)

## Definition

High-level modules should depend on abstractions, not concrete implementations.

## High-Level Module

```csharp
CourierBookingService
```

## Abstractions

```csharp
IDeliveryChargeCalculator

INotificationService

IInvoiceGenerator
```

## Low-Level Modules

```csharp
StandardDeliveryCalculator

ExpressDeliveryCalculator

InternationalDeliveryCalculator

EmailNotificationService

SmsNotificationService

WhatsAppNotificationService

ConsoleInvoiceGenerator
```

### Dependency Injection Example

```csharp
CourierBookingService service =
    new CourierBookingService(
        calculator,
        notificationService,
        invoiceGenerator);
```

CourierBookingService does not know which specific calculator or notification service is being used.

It only works with interfaces.

## Benefit

Components remain loosely coupled and easy to replace.

---

# Conclusion

SmartCourierApp successfully demonstrates all five SOLID principles:

| Principle | Implementation                                                        |
| --------- | --------------------------------------------------------------------- |
| SRP       | Separate classes for calculation, notification, invoice, and booking  |
| OCP       | New delivery types can be added without modifying existing code       |
| LSP       | All calculators can replace one another through the same interface    |
| ISP       | Separate interfaces for notification and invoice services             |
| DIP       | High-level modules depend on abstractions instead of concrete classes |

Following SOLID principles makes the application easier to maintain, test, extend, and scale.
