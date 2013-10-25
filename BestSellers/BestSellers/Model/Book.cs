using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.Net;
using System.IO;

namespace BestSellers
{
    public static class ToTitleCaseExension
    {
        public static string ToTitleCase(this string str)
        {
            string result = str = str.ToLower();
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString(CultureInfo.InvariantCulture).ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
    }

    public class Book
    {
        public string Category { get; set; }
        public string CategoryEncoded { get; set; }
        public string Title { get; set; }
        public string Contributor { get; set; }
        public string Author { get; set; }
        public string Rank { get; set; }
        public string BestSellersDate { get; set; }
        public string PublishedDate { get; set; }
        public string WeeksOnList { get; set; }
        public string RankLastWeek { get; set; }
        public string Description { get; set; }
        public string ContributorNote { get; set; }
        public string Price { get; set; }
        public string AgeGroup { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public string BookReviewLink { get; set; }
        public string FirstChapterLink { get; set; }
        public string SundayReviewLink { get; set; }
        public string ArticleChapterLink { get; set; }

        public string ISBN
        {
            get
            {
                long isbn;
                return Int64.TryParse(ISBN10, out isbn) ? ISBN10 : ISBN13;
            }
        }

        public string AmazonImageUrl
        {
            get { return string.Format("http://images.amazon.com/images/P/{0}.01.ZTZZZZZZ.jpg", ISBN10); }
        }

        public override string ToString()
        {
            return Title + "(" + ISBN + ")";
        }

        public Book() { }

        public static Book Find(string category, string bookId)
        {
            string urlBooks = "http://api.nytimes.com/svc/books/v2/lists.xml?list={0}&isbn={1}&api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

            urlBooks = String.Format(urlBooks, category.Replace(" ", "-"), bookId);

            XDocument loaded = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(urlBooks);
                var response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                loaded = XDocument.Load(stream);
            }
            catch (WebException)
            {
            }

            if (loaded == null)
                return new Book
                 {
                     AgeGroup = string.Empty,
                     ArticleChapterLink = string.Empty,
                     BestSellersDate = string.Empty,
                     BookReviewLink = string.Empty,
                     Category = string.Empty,
                     Contributor = string.Empty,
                     ContributorNote = string.Empty,
                     Description = string.Empty,
                     FirstChapterLink = string.Empty,
                     ISBN10 = string.Empty,
                     ISBN13 = string.Empty,
                     Price = string.Empty,
                     PublishedDate = string.Empty,
                     Publisher = string.Empty,
                     Rank = string.Empty,
                     RankLastWeek = string.Empty,
                     SundayReviewLink = string.Empty,
                     Title = string.Empty,
                     WeeksOnList = string.Empty,
                 };

            var books = from item in loaded.Descendants("book")
                        select new Book
                        {
                            Category = item.Element("display_name").Value,
                            BestSellersDate = item.Element("bestsellers_date").Value,
                            Rank = item.Element("rank").Value,
                            RankLastWeek = item.Element("rank_last_week").Value,
                            WeeksOnList = item.Element("weeks_on_list").Value,
                            PublishedDate = item.Element("published_date").Value,

                            Title = item.Descendants("book_detail").Elements("title").First().Value.ToTitleCase(),
                            Contributor = item.Descendants("book_detail").Elements("contributor").First().Value,
                            ContributorNote = item.Descendants("book_detail").Elements("contributor_note").First().Value,
                            Author = item.Descendants("book_detail").Elements("author").First().Value,
                            ISBN13 = item.Descendants("book_detail").Elements("primary_isbn13").First().Value,
                            ISBN10 = item.Descendants("book_detail").Elements("primary_isbn10").First().Value,
                            Publisher = item.Descendants("book_detail").Elements("publisher").First().Value,
                            AgeGroup = item.Descendants("book_detail").Elements("age_group").First().Value,
                            Price = item.Descendants("book_detail").Elements("price").First().Value,
                            Description = item.Descendants("book_detail").Elements("description").First().Value,

                            BookReviewLink = item.Descendants("review").Elements("book_review_link").First().Value,
                            FirstChapterLink = item.Descendants("review").Elements("first_chapter_link").First().Value,
                            SundayReviewLink = item.Descendants("review").Elements("sunday_review_link").First().Value,
                            ArticleChapterLink = item.Descendants("review").Elements("article_chapter_link").First().Value,
                        };

            return books.Take(1).FirstOrDefault() ?? new Book
            {
                AgeGroup = string.Empty,
                ArticleChapterLink = string.Empty,
                BestSellersDate = string.Empty,
                BookReviewLink = string.Empty,
                Category = string.Empty,
                Contributor = string.Empty,
                ContributorNote = string.Empty,
                Description = string.Empty,
                FirstChapterLink = string.Empty,
                ISBN10 = string.Empty,
                ISBN13 = string.Empty,
                Price = string.Empty,
                PublishedDate = string.Empty,
                Publisher = string.Empty,
                Rank = string.Empty,
                RankLastWeek = string.Empty,
                SundayReviewLink = string.Empty,
                Title = string.Empty,
                WeeksOnList = string.Empty,
            };
        }
    }
}