using ASC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.WebCore.Models
{
    public class Book : BaseEntity
    {
        public Book() { }
        public Book(int bookId, string publisher)
        {
            this.RowKey = bookId.ToString();
            this.PartitionKey = publisher;

        }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string publisher { get; set; }
    }
}
