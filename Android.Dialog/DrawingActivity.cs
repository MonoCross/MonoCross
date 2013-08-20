using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;

namespace Android.Dialog
{
    [Activity(Label = "DrawingActivity", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class DrawingActivity : FragmentActivity
    {
        private const int CONTENT_VIEW_ID = 0x1234;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            FrameLayout frame = new FrameLayout(this);
            frame.Id = CONTENT_VIEW_ID;
            SetContentView(frame, new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent));

            if (bundle == null)
            {
                Fragment newFragment = new DrawingFragment();
                FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
                newFragment.Arguments = new Bundle();
                newFragment.Arguments.PutString(DrawingFragment.DRAWING_LOCATION_INTENT, Intent.GetStringExtra(DrawingFragment.DRAWING_LOCATION_INTENT));
                newFragment.Arguments.PutInt(DrawingFragment.DRAWING_COLOR_INTENT, Intent.GetIntExtra(DrawingFragment.DRAWING_COLOR_INTENT, 0xff00000));
                ft.Add(CONTENT_VIEW_ID, newFragment).Commit();
            }
        }
    }
}