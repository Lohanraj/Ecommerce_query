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
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BorrowBook_ValidInput_ReturnsSuccess()
        {
            //Arrange
            var member = new Member { MemberId = 1, Email = "william@gmail.com", IsActive = true, BorrowedBookCount = 1, IsPremiumMember = false };
            var book = new Book { BookId = 1, BookTitle = "Adventures of Tintin", IsAvailable = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(1)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Book borrowed successfully"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(1), Times.Once);
            mockMember.Verify(x => x.UpdateBorrowedBookCount(1), Times.Once);
            mockNotify.Verify(x => x.SendBorrowNotification("william@gmail.com", "Adventures of Tintin"), Times.Once);
        }

        [Test]
        public void BorrowBook_MemberNotFound_ReturnsError()
        {
            //Arrange
            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(99)).Returns((Member)null);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(99, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Member not found"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            mockMember.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            mockNotify.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void BorrowBook_MemberInactive_ReturnsError()
        {
            //Arrange
            var member = new Member { MemberId = 2, IsActive = false };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(2)).Returns(member);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(2, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Member is not active"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            mockNotify.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void BorrowBook_BookNotFound_ReturnsError()
        {
            //Arrange
            var member = new Member { MemberId = 1, IsActive = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(99)).Returns((Book)null);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 99);

            //Assert
            Assert.That(res, Is.EqualTo("Book not found"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            mockMember.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void BorrowBook_BookNotAvailable_ReturnsError()
        {
            //Arrange
            var member = new Member { MemberId = 1, IsActive = true };
            var book = new Book { BookId = 1, IsAvailable = false };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(1)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Book is not available"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void BorrowBook_NormalMemberHas3Books_ReturnsLimitReached()
        {
            //Arrange
            var member = new Member { MemberId = 1, IsActive = true, BorrowedBookCount = 3, IsPremiumMember = false };
            var book = new Book { BookId = 2, IsAvailable = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(2)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 2);

            //Assert
            Assert.That(res, Is.EqualTo("Borrowing limit reached"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            mockMember.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void BorrowBook_PremiumMemberHas3Books_ReturnsSuccess()
        {
            //Arrange
            var member = new Member { MemberId = 1, Email = "william@gmail.com", IsActive = true, BorrowedBookCount = 3, IsPremiumMember = true };
            var book = new Book { BookId = 1, BookTitle = "Adventures of Tintin", IsAvailable = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(1)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Book borrowed successfully"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(1), Times.Once);
            mockMember.Verify(x => x.UpdateBorrowedBookCount(1), Times.Once);
        }

        [Test]
        public void BorrowBook_PremiumMemberHas5Books_ReturnsLimitReached()
        {
            //Arrange
            var member = new Member { MemberId = 1, IsActive = true, BorrowedBookCount = 5, IsPremiumMember = true };
            var book = new Book { BookId = 1, IsAvailable = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(1)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Borrowing limit reached"));
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            mockNotify.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void BorrowBook_InvalidMemberId_ReturnsError()
        {
            //Arrange
            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(0, 1);

            //Assert
            Assert.That(res, Is.EqualTo("Invalid member id"));
            mockMember.Verify(x => x.GetMemberById(It.IsAny<int>()), Times.Never);
            mockBook.Verify(x => x.GetBookById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void BorrowBook_InvalidBookId_ReturnsError()
        {
            //Arrange
            var member = new Member { MemberId = 1, IsActive = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            var res = service.BorrowBook(1, 0);

            //Assert
            Assert.That(res, Is.EqualTo("Invalid book id"));
            mockBook.Verify(x => x.GetBookById(It.IsAny<int>()), Times.Never);
            mockBook.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void BorrowBook_Success_NotificationCalledWithCorrectValues()
        {
            //Arrange
            var member = new Member { MemberId = 1, Email = "john@gmail.com", IsActive = true, BorrowedBookCount = 1, IsPremiumMember = false };
            var book = new Book { BookId = 1, BookTitle = "CSharp Fundamentals", IsAvailable = true };

            var mockMember = new Mock<IMemberRepository>();
            var mockBook = new Mock<IBookRepository>();
            var mockNotify = new Mock<INotificationService>();

            mockMember.Setup(x => x.GetMemberById(1)).Returns(member);
            mockBook.Setup(x => x.GetBookById(1)).Returns(book);

            var service = new LibraryService(mockMember.Object, mockBook.Object, mockNotify.Object);

            //Act
            service.BorrowBook(1, 1);

            //Assert
            mockNotify.Verify(x => x.SendBorrowNotification("john@gmail.com", "CSharp Fundamentals"), Times.Once);
        }
    }
}