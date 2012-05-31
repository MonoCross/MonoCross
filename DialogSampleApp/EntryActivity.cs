using Android.App;
using Android.Dialog;
using Android.OS;
using Android.Views;

namespace DialogSampleApp
{
    [Activity(Label = "My Activity", Theme = "@android:style/Theme.Black.NoTitleBar", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class EntryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var dialogListView = new DialogListView(this);
            SetContentView(dialogListView);
            dialogListView.Root = InitializeRoot();
        }

        RootElement InitializeRoot()
        {
            RootElement root = new RootElement("Elements");
            Section section = new Section("Section");
            root.Add(section);

            section.Add(new EntryElement("Label 1", "Value 1"));
            section.Add(new EntryElement("Label 2", "Value 2"));
            section.Add(new EntryElement("Label 3", "Value 3"));
            section.Add(new EntryElement("Label 4", "Value 4"));
            section.Add(new EntryElement("Label 5", "Value 5"));
            section.Add(new EntryElement("Label 6", "Value 6"));

            section.Add(new WebContentElement("http://www.google.com")
            {
                WebContent = "<html><body><h1>My First Heading</h1><p>My first paragraph.</p></body></html>"
            });

            //section.Add(new EntryElement("Label 7", "Value 7"));

            return root;
        }

        /*
        void InitializeRoot()
        {
            RootElement root = new RootElement("Elements");
            Section section = new Section("List of Elements");
            root.Add(section);
            var field = new EntryElement("Email", "t.le.d@xxx.com", (int)DroidResources.ElementLayout.dialog_textfieldbelow) {
                Lines = 3
            };
            section.Add(field);
            //section.Add(new ButtonElement("Submit", null));

            ListView.AddFooterView(getFooterButton(this));

            Root = root;
        }

        View getFooterButton(Context context)
        {
            //var button = formLayer.ActionButton;
            var buttonctrl = (context as Activity).LayoutInflater.Inflate(context.Resources.GetIdentifier("dialog_button", "layout", context.PackageName), this.ListView, false);

            var buttonView = buttonctrl.FindViewById<Button>(context.Resources.GetIdentifier("dialog_Button", "id", context.PackageName));
            //buttonView.Tag = new SubmitButtonTagData(formLayer, dialogFormControl);
            //buttonView.Click += new EventHandler(Submit_Clicked);
            return buttonctrl;
        }
        */
    }
}