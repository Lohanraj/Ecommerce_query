namespace LibraryMembershipApp.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int BorrowedBookCount { get; set; }
        public bool IsPremiumMember { get; set; }
    }
}