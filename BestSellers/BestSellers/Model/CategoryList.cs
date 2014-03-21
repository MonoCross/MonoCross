using System.Collections.Generic;

namespace BestSellers
{

    public class Category
    {
        public string DisplayName { get; set; }
        public string ListNameEncoded
        {
            get { return DisplayName.ToLower().Replace(' ', '-'); }
        }
        //public string OldestPublishedDate { get; set; }
        //public string NewestPublishedDate { get; set; }
        //public string Updated { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class CategoryList : List<Category>
    {
    }
}
