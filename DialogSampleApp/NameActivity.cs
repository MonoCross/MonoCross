using Android.App;
using Android.Dialog;
using Android.OS;

namespace DialogSampleApp
{
    [Activity(Label = "My Activity", Theme = "@android:style/Theme.Black.NoTitleBar")]
    public class NameActivity : Activity {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // create from view
            var dialogListView = new DialogListView(this);
            SetContentView(dialogListView);

            RootElement root = new RootElement("Elements");
            Section section = new Section();
            root.Add(section);

            foreach (var item in _names) {
                section.Add(new StringElement(item.LastName, item.FirstName, Resource.Layout.custom_string_element));
            }

            dialogListView.Root = root;
        }

        class Name {
            public Name(string first, string last)
            {
                FirstName = first;
                LastName = last;
            }

            public string FirstName;
            public string LastName;
        }

        private static Name[] _names = {
            new Name("Kristina", "Chung"),
            new Name("Paige", "Chen"),
            new Name("Sherri", "Melton"),
            new Name("Gretchen", "Hill"),
            new Name("Karen", "Puckett"),
            new Name("Patrick", "Song"),
            new Name("Elsie", "Hamilton"),
            new Name("Hazel", "Bender"),
            new Name("Malcolm", "Wagner"),
            new Name("Dolores", "McLaughlin"),
            new Name("Francis", "McNamara"),
            new Name("Sandy", "Raynor"),
            new Name("Marion", "Moon"),
            new Name("Beth", "Woodard"),
            new Name("Julia", "Desai"),
            new Name("Jerome", "Wallace"),
            new Name("Neal", "Lawrence"),
            new Name("Jean", "Griffin"),
            new Name("Kristine", "Dougherty"),
            new Name("Crystal", "Powers"),
            new Name("Alex", "May"),
            new Name("Eric", "Steele"),
            new Name("Wesley", "Teague"),
            new Name("Franklin", "Vick"),
            new Name("Claire", "Gallagher"),
            new Name("Marian", "Solomon"),
            new Name("Marcia", "Walsh"),
            new Name("Dwight", "Monroe"),
            new Name("Wayne", "Connolly"),
            new Name("Stephanie", "Hawkins"),
            new Name("Neal", "Middleto"),
            new Name("Gretchen", "Goldstein"),
            new Name("Tim", "Watts"),
            new Name("Jerome", "Johnston"),
            new Name("Shelley", "Weeks"),
            new Name("Priscilla", "Wilkerson"),
            new Name("Elsie", "Barton"),
            new Name("Beth", "Walton"),
            new Name("Erica", "Hall"),
            new Name("Douglas", "Ross"),
            new Name("Donald", "Chung"),
            new Name("Katherine", "Bender"),
            new Name("Paul", "Woods"),
            new Name("Patricia", "Mangum"),
            new Name("Lois", "Joseph"),
            new Name("Louis", "Rosenthal"),
            new Name("Christina", "Bowden"),
            new Name("Darlene", "Barton"),
            new Name("Harvey", "Underwood"),
            new Name("William", "Jones"),
            new Name("Frederick", "Baker"),
            new Name("Shirley", "Merritt"),
            new Name("Jason", "Cross"),
            new Name("Judith", "Cooper"),
            new Name("Gretchen", "Holmes"),
            new Name("Don", "Sharpe"),
            new Name("Glenda", "Morgan"),
            new Name("Scott", "Hoyle"),
            new Name("Pat", "Allen"),
            new Name("Michelle", "Rich"),
        };
    }
}