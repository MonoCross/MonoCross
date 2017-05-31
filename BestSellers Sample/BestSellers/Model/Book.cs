using System;
using System.Text.RegularExpressions;

namespace BestSellers
{
    public static class ToTitleCaseExension
    {
        public static string ToTitleCase(this string str)
        {
            return str == null ? null : Regex.Replace(str, @"\w+", (m) =>
            {
                string tmp = m.Value;
                return char.ToUpper(tmp[0]) + tmp.Substring(1, tmp.Length - 1).ToLower();
            });
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

        public Book() 
        { 
			AgeGroup = string.Empty;
			ArticleChapterLink = string.Empty;
			BestSellersDate = string.Empty;
			BookReviewLink = string.Empty;
			Category = string.Empty;
			Contributor = string.Empty;
			ContributorNote = string.Empty;
			Description = string.Empty;
			FirstChapterLink = string.Empty;
			ISBN10 = string.Empty;
			ISBN13 = string.Empty;
			Price = string.Empty;
			PublishedDate = string.Empty;
			Publisher = string.Empty;
			Rank = string.Empty;
			RankLastWeek = string.Empty;
			SundayReviewLink = string.Empty;
			Title = string.Empty;
			WeeksOnList = string.Empty;
        }
    }
}