using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Borrowing
    {
        public Book Book { get; set; }
        public Member Member { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public Borrowing(Book book, Member member, DateTime borrowDate, DateTime returnDate)
        {
            Book = book;
            Member = member;
            BorrowDate = borrowDate;
            ReturnDate = returnDate;
        }
    }
}
