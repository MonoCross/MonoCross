using System;
using Android.App;
using Android.Content.PM;
using Android.Dialog;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace DialogSampleApp
{
    [Activity(Label = "MonoDroidDialogApp",
        WindowSoftInputMode = SoftInput.AdjustPan,
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTop,
        Theme = "@android:style/Theme.Light.NoTitleBar")]
    public class DialogListViewActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // create from layout file
            SetContentView(Resource.Layout.form_with_buttons);
            var dialogListView = FindViewById<DialogListView>(Android.Resource.Id.List);
            dialogListView.Root = InitializeRoot();
        }

        private RootElement InitializeRoot()
        {
            var bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon);
            var image = new ImageView(this);
            image.SetImageBitmap(bitmap);

            return new RootElement("Elements")
            {
                new Section("Element w/Format Overrides")
                {
                    new CheckboxElement("CheckboxElement", true, "", (int)DroidResources.ElementLayout.dialog_boolfieldsubright),
                    new StringElement("String Element", "Value", (int)DroidResources.ElementLayout.dialog_labelfieldbelow),
                    new EntryElement("EntryElement", string.Empty, (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Plain" },
                    new EntryElement("PasswordEntryElement", "Va", (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Password", Password = true, },
                    new EntryElement("EntryElement2", "Val", (int)DroidResources.ElementLayout.dialog_textfieldbelow) { Hint = "Plain3" },
                },
                new Section("Section")
                {
                    new BooleanElement("BooleanElement", true),
                    new StringElement("StringElement", "Value"),
                    new EntryElement("EntryElement", "") { Hint = "Pain 2" },
                    new EntryElement("PasswordEntryElement", string.Empty) { Hint = "Password 2", Password = true, },
                    new DateTimeElement("DateTimeElement", null),
                    new DateElement("DateElement", DateTime.Now),
                    new TimeElement("TimeElement", DateTime.Now),
                    new CheckboxElement("CheckboxElement", true),
                    new HtmlElement("HtmlElement (Link)", "http://www.google.com"),
                    new ImageElement(image),
                    new StringElement("MultiLineElement", "The quick brown fox jumped over the lazy horse, the quick brown fox jumped over the lazy horse"),
                    new FloatElement("Range"),
                },
                new Section("Groups")
                {
                    new RootElement ("Radio Group",  new Android.Dialog.RadioGroup ("dessert", 2))
                    {
                        new Section
                        {
                            new RadioElement ("Ice Cream Sandwich", "dessert"),
                            new RadioElement ("Honeycomb", "dessert"),
                            new RadioElement ("Gingerbread", "dessert")
                        },
                        new Section
                        {
                            new RadioElement ("Frozen Yogurt", "dessert"),
                            new RadioElement ("Eclair", "dessert"),
                            new RadioElement ("Donut", "dessert")
                        },
                    }
                },
            };
        }
    }
}