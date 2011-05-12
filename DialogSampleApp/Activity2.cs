using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

using MonoDroid.Dialog;
using Android.Graphics;

namespace DialogSampleApp
{
    [Activity(Label = "MonoDroidDialogApp",
        WindowSoftInputMode = SoftInput.AdjustPan,
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTop)]
    public class Activity2 : DialogActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            InitializeRoot();

            base.OnCreate(bundle);
        }

        void InitializeRoot()
        {
            Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon);

            this.Root = new RootElement("Elements")
            {
                new Section("Element w/Format Overrides")
                {
                    new CheckboxElement("CheckboxElement", true, "", (int)DroidResources.ElementLayout.dialog_boolfieldsubright),
                    new StringElement("String Element", "Value", (int)DroidResources.ElementLayout.dialog_labelfieldbelow),
                    new EntryElement("EntryElement", "", (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Plain" },
                    new EntryElement("PasswordEntryElement", "Va", (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Password", Password = true },
                    new EntryElement("EntryElement2", "Val", (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Plain3" },
                },
                new Section("Section")
                {
                    new BooleanElement("BooleanElement", true),
                    new StringElement("StringElement", "Value"),
                    new EntryElement("EntryElement", "") { Hint = "Pain 2" },
                    new EntryElement("PasswordEntryElement", "") { Hint = "Password 2", Password = true },
                    new DateTimeElement("DateTimeElement", DateTime.Now),
                    new DateElement("DateElement", DateTime.Now),
                    new TimeElement("TimeElement", DateTime.Now),
                    new CheckboxElement("CheckboxElement", true),
					new HtmlElement("HtmlElement (Link)","http://www.google.com"),
                    //new ImageElement(bitmap),
                    new MultilineElement("MultiLineElement", "The quick brown fox jumped over the lazy horse, the quick brown fox jumped over the lazy horse"),
                    new FloatElement("Range"),
                },
                new Section("Groups")
                {
                    new RootElement ("Radio Group",  new MonoDroid.Dialog.RadioGroup ("desert", 2))
                    {
                        new Section ()
                        {
                            new RadioElement ("Ice Cream", "desert"),
                            new RadioElement ("Milkshake", "desert"),
                            new RadioElement ("Chocolate Cake", "desert")
                        },
                        new Section ()
                        {
                            new RadioElement ("Ice Cream", "desert"),
                            new RadioElement ("Milkshake", "desert"),
                            new RadioElement ("Chocolate Cake", "desert")
                        }
                    }
                },
            };
        }
    }
}

