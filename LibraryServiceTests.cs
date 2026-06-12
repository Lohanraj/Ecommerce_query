using Moq;
using NUnit.Framework;
using LibraryMembershipApp.Interfaces;
using LibraryMembershipApp.Models;
using LibraryMembershipApp.Services;

namespace LibraryMembershipApp.Tests
{
    [TestFixture]
    public class LibraryServiceTests
    {
        // ── Mock fields ──────────────────────────────────────────────────────────
        private Mock<IMemberRepository> _mockMemberRepository;
        private Mock<IBookRepository> _mockBookRepository;
        private Mock<INotificationService> _mockNotificationService;
        private LibraryService _libraryService;

        // ── Test constants ───────────────────────────────────────────────────────
        private const int ValidMemberId = 1;
        private const int ValidBookId   = 10;

        // ── SetUp ────────────────────────────────────────────────────────────────
        [SetUp]
        public void SetUp()
        {
            _mockMemberRepository    = new Mock<IMemberRepository>();
            _mockBookRepository      = new Mock<IBookRepository>();
            _mockNotificationService = new Mock<INotificationService>();

            _libraryService = new LibraryService(
                _mockMemberRepository.Object,
                _mockBookRepository.Object,
                _mockNotificationService.Object);
        }

        // ────────────────────────────────────────────────────────────────────────
        // Helper builders
        // ────────────────────────────────────────────────────────────────────────
        private static Member ActiveMember(int borrowed = 0, bool premium = false) => new Member
        {
            MemberId          = ValidMemberId,
            MemberName        = "Alice",
            Email             = "alice@example.com",
            IsActive          = true,
            BorrowedBookCount = borrowed,
            IsPremiumMember   = premium
        };

        private static Book AvailableBook() => new Book
        {
            BookId    = ValidBookId,
            BookTitle = "Clean Code",
            AuthorName = "Robert C. Martin",
            IsAvailable = true
        };

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 11 — Success scenario
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenAllConditionsAreValid_ShouldReturnSuccessMessage()
        {
            // Arrange
            var member = ActiveMember(borrowed: 1);
            var book   = AvailableBook();

            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(member);
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(book);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Book borrowed successfully"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(ValidBookId), Times.Once);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(ValidMemberId), Times.Once);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(member.Email, book.BookTitle), Times.Once);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 12 — Member not found
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenMemberDoesNotExist_ShouldReturnMemberNotFound()
        {
            // Arrange
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns((Member?)null);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Member not found"));
            _mockBookRepository.Verify(r => r.GetBookById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 13 — Member inactive
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenMemberIsInactive_ShouldReturnMemberIsNotActive()
        {
            // Arrange
            var inactiveMember = new Member
            {
                MemberId  = ValidMemberId,
                MemberName = "Bob",
                Email     = "bob@example.com",
                IsActive  = false,
                BorrowedBookCount = 0
            };
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(inactiveMember);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Member is not active"));
            _mockBookRepository.Verify(r => r.GetBookById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 14 — Book not found
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenBookDoesNotExist_ShouldReturnBookNotFound()
        {
            // Arrange
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(ActiveMember());
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns((Book?)null);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Book not found"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 15 — Book not available
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenBookIsNotAvailable_ShouldReturnBookIsNotAvailable()
        {
            // Arrange
            var unavailableBook = new Book
            {
                BookId    = ValidBookId,
                BookTitle = "The Pragmatic Programmer",
                AuthorName = "Andrew Hunt",
                IsAvailable = false
            };
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(ActiveMember());
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(unavailableBook);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Book is not available"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 16 — Borrowing limit reached (normal member, 3 books)
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenBorrowingLimitReached_ShouldReturnBorrowingLimitReached()
        {
            // Arrange
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId))
                                  .Returns(ActiveMember(borrowed: 3));
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(AvailableBook());

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Borrowing limit reached"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 17 — Invalid member id
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenMemberIdIsInvalid_ShouldReturnInvalidMemberId()
        {
            // Arrange
            // No setup needed — validation happens before any repository call

            // Act
            var result = _libraryService.BorrowBook(0, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Invalid member id"));
            _mockMemberRepository.Verify(r => r.GetMemberById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.GetBookById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 18 — Invalid book id
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenBookIdIsInvalid_ShouldReturnInvalidBookId()
        {
            // Arrange
            // No setup needed — validation happens before any repository call

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, -5);

            // Assert
            Assert.That(result, Is.EqualTo("Invalid book id"));
            _mockMemberRepository.Verify(r => r.GetMemberById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.GetBookById(It.IsAny<int>()), Times.Never);
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 19 — Premium member rules
        // ════════════════════════════════════════════════════════════════════════

        [Test]
        public void BorrowBook_WhenNormalMemberHasThreeBooks_ShouldReturnBorrowingLimitReached()
        {
            // Arrange
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId))
                                  .Returns(ActiveMember(borrowed: 3, premium: false));
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(AvailableBook());

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Borrowing limit reached"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void BorrowBook_WhenPremiumMemberHasThreeBooks_ShouldAllowBorrowing()
        {
            // Arrange
            var member = ActiveMember(borrowed: 3, premium: true);
            var book   = AvailableBook();
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(member);
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(book);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Book borrowed successfully"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(ValidBookId), Times.Once);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(ValidMemberId), Times.Once);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(member.Email, book.BookTitle), Times.Once);
        }

        [Test]
        public void BorrowBook_WhenPremiumMemberHasFiveBooks_ShouldReturnBorrowingLimitReached()
        {
            // Arrange
            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId))
                                  .Returns(ActiveMember(borrowed: 5, premium: true));
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(AvailableBook());

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Borrowing limit reached"));
            _mockBookRepository.Verify(r => r.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _mockMemberRepository.Verify(r => r.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        // ════════════════════════════════════════════════════════════════════════
        // Assignment 20 — Notification sent with correct values
        // ════════════════════════════════════════════════════════════════════════
        [Test]
        public void BorrowBook_WhenSuccessful_ShouldSendNotificationWithCorrectEmailAndBookTitle()
        {
            // Arrange
            var member = new Member
            {
                MemberId          = ValidMemberId,
                MemberName        = "Carol",
                Email             = "carol@library.com",
                IsActive          = true,
                BorrowedBookCount = 0,
                IsPremiumMember   = false
            };
            var book = new Book
            {
                BookId      = ValidBookId,
                BookTitle   = "Design Patterns",
                AuthorName  = "Gang of Four",
                IsAvailable = true
            };

            _mockMemberRepository.Setup(r => r.GetMemberById(ValidMemberId)).Returns(member);
            _mockBookRepository.Setup(r => r.GetBookById(ValidBookId)).Returns(book);

            // Act
            var result = _libraryService.BorrowBook(ValidMemberId, ValidBookId);

            // Assert
            Assert.That(result, Is.EqualTo("Book borrowed successfully"));
            _mockNotificationService.Verify(
                n => n.SendBorrowNotification("carol@library.com", "Design Patterns"),
                Times.Once,
                "Notification must be sent with the member's email and the borrowed book's title.");
        }
    }
}
