﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using MonoCross.Navigation;
using System.Diagnostics;

namespace BestSellers.Controllers
{
    class BookListController : MXController<BookList>
    {
        const string UrlBooklist = "http://api.nytimes.com/svc/books/v2/lists/{0}.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

        public override string Load(Dictionary<string, string> parameters)
        {
            string category = parameters.ContainsKey("Category") ? parameters["Category"] : "BookList";

            Model = new BookList { Category = category, };

            Model.AddRange(BookList(category));

            if (Model.Count > 0)
                Model.CategoryDisplayName = Model[0].Category;

            return ViewPerspective.Read;
        }

        public static List<Book> BookList(string category)
        {
	        List<Book> books = new List<Book>();
            string urlBooks = String.Format(UrlBooklist, category);
            var request = (HttpWebRequest)WebRequest.Create(urlBooks);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();

            using (XmlReader reader = XmlReader.Create(stream))
            {
                try
                {
                    bool hasNextResult = reader.ReadToFollowing("book");
                    while (hasNextResult)
                    {
                        var book = new Book();
                        book.CategoryEncoded = category;
                        reader.ReadToDescendant("list_name");
                        book.Category = reader.ReadInnerXml();
                        reader.ReadToFollowing("bestsellers_date");
                        book.BestSellersDate = reader.ReadInnerXml();
                        book.PublishedDate = reader.ReadInnerXml();
                        reader.ReadToFollowing("rank");
                        book.Rank = reader.ReadInnerXml();
                        book.RankLastWeek = reader.ReadInnerXml();
                        book.WeeksOnList = reader.ReadInnerXml();
                        reader.ReadToDescendant("isbns");
                        reader.ReadToFollowing("isbn10");
                        book.ISBN10 = reader.ReadInnerXml();
                        book.ISBN13 = reader.ReadInnerXml();
                        reader.ReadToFollowing("book_details");
                        reader.ReadToFollowing("title");
                        book.Title = reader.ReadInnerXml();
                        book.Description = reader.ReadInnerXml();
                        book.Contributor = reader.ReadInnerXml();
                        book.Author = reader.ReadInnerXml();
                        reader.ReadToFollowing("price");
                        book.Price = reader.ReadInnerXml();
                        reader.ReadToFollowing("publisher");
                        book.Publisher = reader.ReadInnerXml();
                        reader.ReadToFollowing("book_image");
                         
						reader.ReadToFollowing("book_review_link");   
						book.BookReviewLink = reader.ReadInnerXml();
                        book.FirstChapterLink = reader.ReadInnerXml();
                        book.SundayReviewLink = reader.ReadInnerXml();
                        book.ArticleChapterLink = reader.ReadInnerXml();

                        books.Add(book);
                        
                        hasNextResult = reader.ReadToFollowing("book");
                    }
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine("Exception:\r\n" + e); }
            }    
            return books;
        }

    }
}
