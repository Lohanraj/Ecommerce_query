namespace LibraryMembershipApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
