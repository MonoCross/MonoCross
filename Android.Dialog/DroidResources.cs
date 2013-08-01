using System;
using System.Globalization;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace Android.Dialog
{
    public static class DroidResources
    {
        public static View LoadFloatElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out SeekBar slider, out ImageView left, out ImageView right)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                slider = layout.FindViewById<SeekBar>(Resource.Id.dialog_SliderField);
                left = layout.FindViewById<ImageView>(Resource.Id.dialog_ImageLeft);
                right = layout.FindViewById<ImageView>(Resource.Id.dialog_ImageRight);
            }
            else
            {
                label = null;
                slider = null;
                left = right = null;
            }
            return layout;
        }


        private static View LoadLayout(Context context, ViewGroup parent, int layoutId)
        {
            try
            {
                return LayoutInflater.FromContext(context).Inflate(layoutId, parent, false);
            }
            catch (InflateException ex)
            {
                Log.Error("Android.Dialog", "Inflate failed: " + ex.Cause.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Android.Dialog", "LoadLayout failed: " + ex.Message);
            }
            return null;
        }

        public static View LoadStringElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out TextView value)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                value = layout.FindViewById<TextView>(Resource.Id.dialog_ValueField);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadStringElementLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                label = null;
                value = null;
            }
            return layout;
        }

        public static View LoadButtonLayout(Context context, View convertView, ViewGroup parent, int layoutId, out Button button)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                button = layout.FindViewById<Button>(Resource.Id.dialog_Button);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadButtonLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                button = null;
            }
            return layout;
        }

        public static View LoadMultilineElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out EditText value)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                value = layout.FindViewById<EditText>(Resource.Id.dialog_ValueField);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadMultilineElementLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                value = null;
            }
            return layout;
        }

        public static View LoadBooleanElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out TextView subLabel, out View value)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                value = layout.FindViewById<View>(Resource.Id.dialog_BoolField);
                subLabel = layout.FindViewById<TextView>(Resource.Id.dialog_LabelSubtextField);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadBooleanElementLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                label = null;
                value = null;
                subLabel = null;
            }
            return layout;
        }

        public static View LoadStringEntryLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out EditText value)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                value = layout.FindViewById<EditText>(Resource.Id.dialog_ValueField);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadStringEntryLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                label = null;
                value = null;
            }
            return layout;
        }

        public static View LoadHtmlLayout(Context context, View convertView, ViewGroup parent, int layoutId, out WebView webView)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                webView = layout.FindViewById<WebView>(Resource.Id.dialog_HtmlField);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadHtmlLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                webView = null;
            }
            return layout;
        }

        public static View LoadEntryButtonLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out EditText value, out ImageButton button)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                value = layout.FindViewById<EditText>(Resource.Id.dialog_ValueField);
                button = layout.FindViewById<ImageButton>(Resource.Id.dialog_Button);
            }
            else
            {
                Log.Error("Android.Dialog", "LoadStringEntryLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                label = null;
                value = null;
                button = null;
            }
            return layout;
        }

        public static View LoadAchievementsElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView caption, out TextView description, out TextView percentageComplete, out ImageView achivementImage)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout == null)
            {
                Log.Error("Android.Dialog", "LoadAchievementsElementLayout: Failed to load resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                achivementImage = null;
                caption = null;
                description = null;
                percentageComplete = null;
            }
            else
            {
                achivementImage = layout.FindViewById<ImageView>(Resource.Id.dialog_ImageRight);
                caption = layout.FindViewById<TextView>(Resource.Id.dialog_LabelField);
                description = layout.FindViewById<TextView>(Resource.Id.dialog_LabelSubtextField);
                percentageComplete = layout.FindViewById<TextView>(Resource.Id.dialog_LabelPercentageField);
            }
            return layout;
        }
    }
}