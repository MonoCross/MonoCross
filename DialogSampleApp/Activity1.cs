using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Sax;
using Android.Views;
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

            var lv = new ListView(this) {Adapter = null};

            var elements = new List<Element>
                               {
                                   new BooleanElement(this, "Push my button", true),
                                   new BooleanElement(this, "Push this too", false)
                               };

            var da = new DialogAdapter(this,elements);
            lv.Adapter = da;
            
            SetContentView(lv);
        }
    }
}

