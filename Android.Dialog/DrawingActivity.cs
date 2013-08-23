using Android.OS;
using Android.Support.V4.App;
using Android.Widget;

namespace Android.Dialog
{
    public class DrawingActivity : FragmentActivity
    {
        private const int ContentViewID = 0x1234;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var frame = new FrameLayout(this) { Id = ContentViewID, };
            SetContentView(frame, new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent));

            if (bundle != null) return;
            Fragment newFragment = new DrawingFragment();
            var ft = SupportFragmentManager.BeginTransaction();
            newFragment.Arguments = new Bundle();
            newFragment.Arguments.PutString(DrawingFragment.DRAWING_LOCATION_INTENT, Intent.GetStringExtra(DrawingFragment.DRAWING_LOCATION_INTENT));
            newFragment.Arguments.PutInt(DrawingFragment.DRAWING_COLOR_INTENT, Intent.GetIntExtra(DrawingFragment.DRAWING_COLOR_INTENT, 0xff00000));
            ft.Add(ContentViewID, newFragment).Commit();
        }
    }
}