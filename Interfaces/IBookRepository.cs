using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Interfaces
{
    public interface IBookRepository
    {
        Book? GetBookById(int bookId);
        void MarkBookAsBorrowed(int bookId);
    }
}
