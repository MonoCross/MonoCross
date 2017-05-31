using System.Collections.Generic;

namespace BestSellers
{
    public class BookList : List<Book>
    {
        public string Category { get; set; }
        public string CategoryDisplayName { get; set; }
    }
}