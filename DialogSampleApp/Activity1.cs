using Android.App;
using Android.OS;
using Android.Widget;
using MonoDroid.Dialog;

namespace DialogSampleApp
{
    [Activity(Label = "MD.D Sample", MainLauncher = true)]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var root = new RootElement(this, "Test Root Elem")
                           {
                               new Section(this, "Test Header", "Test Footer")
                                   {
                                       new BooleanElement(this, "Push my button", true),
                                       new BooleanElement(this, "Push this too", false),
                                       new StringElement(this, "Text label", "The Value"),
                                   },
                               new Section(this, "Part II")
                                   {
                                       new StringElement(this, "This is the String Element", "The Value"),
                                       new CheckboxElement(this, "Check this out", true)
                                   }
                           };

            var da = new DialogAdapter(root);

            var lv = new ListView(this) {Adapter = da};

            SetContentView(lv);
        }
    }
}