using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Interfaces
{
    public interface IMemberRepository
    {
        Member? GetMemberById(int memberId);
        void UpdateBorrowedBookCount(int memberId);
    }
}
