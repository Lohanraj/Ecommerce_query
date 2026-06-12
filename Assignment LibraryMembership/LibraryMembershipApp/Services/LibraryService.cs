using LibraryMembershipApp.Interfaces;
using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Services
{
    public class LibraryService
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IBookRepository _bookRepo;
        private readonly INotificationService _notify;

        public LibraryService(IMemberRepository memberRepo, IBookRepository bookRepo, INotificationService notify)
        {
            _memberRepo = memberRepo;
            _bookRepo = bookRepo;
            _notify = notify;
        }

        public string BorrowBook(int memberId, int bookId)
        {
            // validate ids first
            if (memberId <= 0)
                return "Invalid member id";

            if (bookId <= 0)
                return "Invalid book id";

            var member = _memberRepo.GetMemberById(memberId);
            if (member == null)
                return "Member not found";

            if (!member.IsActive)
                return "Member is not active";

            var book = _bookRepo.GetBookById(bookId);
            if (book == null)
                return "Book not found";

            if (!book.IsAvailable)
                return "Book is not available";

            // premium gets 5, normal gets 3
            int limit = member.IsPremiumMember ? 5 : 3;

            if (member.BorrowedBookCount >= limit)
                return "Borrowing limit reached";

            _bookRepo.MarkBookAsBorrowed(bookId);
            _memberRepo.UpdateBorrowedBookCount(memberId);
            _notify.SendBorrowNotification(member.Email, book.BookTitle);

            return "Book borrowed successfully";
        }
    }
}