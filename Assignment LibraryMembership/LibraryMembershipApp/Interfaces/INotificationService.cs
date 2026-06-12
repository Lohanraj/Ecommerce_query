namespace LibraryMembershipApp.Interfaces
{
    public interface INotificationService
    {
        void SendBorrowNotification(string email, string bookTitle);
    }
}