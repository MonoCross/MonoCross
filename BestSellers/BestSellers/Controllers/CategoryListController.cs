using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using MonoCross.Navigation;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace BestSellers.Controllers
{
    public class CategoryListController : MXController<CategoryList>
    {
        const string UrlCategories = "http://api.nytimes.com/svc/books/v2/lists/names.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

        public override string Load(Dictionary<string, string> parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(UrlCategories);

            Model = new CategoryList();
            var response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (stream == null)
                throw new Exception("Failed to connect the New Yorw Times Best Sellers Serivce");

            CategoryList categories = new CategoryList();
            using (XmlReader reader = XmlReader.Create(stream))
            {
                try
                { 
                    bool hasNextResult = reader.ReadToFollowing("result");
                    while (hasNextResult)
                    {
                        var c = new Category();
                        reader.ReadToDescendant("list_name");
                        c.DisplayName = reader.ReadInnerXml();
                        categories.Add(c);

                        hasNextResult = reader.ReadToNextSibling("result");
                    }
                }
                catch (Exception e) { System.Diagnostics.Debug.WriteLine("Exception:\r\n" + e); }
            }
            Model.AddRange(categories);

            return ViewPerspective.Read;
        }
    }
}
