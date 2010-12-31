using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using MonoDroid.Dialog;
using Element = MonoDroid.Dialog.Element;

namespace DialogSampleApp
{
    [Activity(Label = "MD.D Sample", MainLauncher = true)]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var elements = new List<Element>
                               {
                                   new BooleanElement(this, "Push my button", true),
                                   new BooleanElement(this, "Push this too", false),
                                   new StringElement(this,"This is the String Element","The Value")
                               };

            var da = new DialogAdapter(this, elements);

            var lv = new ListView(this) {Adapter = da};
            
            SetContentView(lv);
        }
    }
}