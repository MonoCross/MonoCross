using System;
using System.Linq;
using System.Collections.Generic;
using MonoCross.Navigation;

namespace BestSellers.Controllers
{
    class BookController : MXController<Book>
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            string category = parameters.ContainsKey("Category") ? parameters["Category"] : string.Empty;
            string book = parameters.ContainsKey("Book") ? parameters["Book"] : string.Empty;

            Model = FindBook(category, book);

			return ViewPerspective.Read;
        }

        public static Book FindBook(string category, string bookId)
        {
            string urlBooks = "http://api.nytimes.com/svc/books/v2/lists.xml?list={0}&isbn={1}&api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

            urlBooks = String.Format(urlBooks, category.Replace(" ", "-"), bookId);

            List<Book> books = BookListController.BookList(category);

			var results = books.Where(b => b.ISBN == bookId);
            
            if (results == null || results.Count() < 1) return new Book();
			return results.FirstOrDefault();
        }
    }
}