using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class FloatElement : Element, SeekBar.IOnSeekBarChangeListener
    {
        private const int precision = 10000000;
        public bool ShowCaption;
        private int _value, _maxValue, _minValue;

        public float Value
        {
            get { return (float)_value / precision; }
            set { _value = (int)(value * precision); }
        }

        public float MaxValue
        {
            get { return (float)_maxValue / precision; }
            set { _maxValue = (int)(value * precision); }
        }

        public float MinValue
        {
            get { return (float)_minValue / precision; }
            set { _minValue = (int)(value * precision); }
        }

        public Bitmap Left;
        public Bitmap Right;

        public FloatElement(string caption)
            : this(caption, Resource.Layout.dialog_floatimage)
        {
            Value = 0;
            MinValue = 0;
            MaxValue = 1;
        }

        public FloatElement(string caption, int layoutId)
            : base(caption, layoutId)
        {
            Value = 0;
            MinValue = 0;
            MaxValue = 1;
        }

        public FloatElement(Bitmap left, Bitmap right, int value)
            : this(left, right, value, Resource.Layout.dialog_floatimage)
        {
        }

        public FloatElement(Bitmap left, Bitmap right, int value, int layoutId)
            : base(string.Empty, layoutId)
        {
            Left = left;
            Right = right;
            MinValue = 0;
            MaxValue = 1;
            Value = value;
        }

        public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            TextView label;
            SeekBar slider;
            ImageView left;
            ImageView right;

            View view = DroidResources.LoadFloatElementLayout(context, convertView, parent, LayoutId, out label, out slider, out left, out right);

            if (view != null)
            {
                if (left != null)
                {
                    if (Left != null)
                        left.SetImageBitmap(Left);
                    else
                        left.Visibility = ViewStates.Gone;
                }
                if (right != null)
                {
                    if (Right != null)
                        right.SetImageBitmap(Right);
                    else
                        right.Visibility = ViewStates.Gone;
                }
                slider.Max = _maxValue - _minValue;
                slider.Progress = _value - _minValue;
                slider.SetOnSeekBarChangeListener(this);
                if (label != null)
                {
                    if (ShowCaption)
                        label.Text = Caption;
                    else
                        label.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                Util.Log.Error("FloatElement", "GetView failed to load template view");
            }

            return view;
        }

        public override string Summary()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            _value = _minValue + progress;
        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}