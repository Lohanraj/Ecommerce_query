namespace LibraryMembershipApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get;set; }
        public bool IsAvailable { get; set; }
    }
}