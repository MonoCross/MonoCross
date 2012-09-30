using System;
using System.Collections.Generic;
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
        public enum ElementLayout
        {
            dialog_achievements,
            dialog_boolfieldleft,
            dialog_boolfieldright,
            dialog_boolfieldsubleft,
            dialog_boolfieldsubright,

            dialog_button,
            dialog_datefield,
            dialog_fieldsetlabel,
            dialog_labelfieldbelow,
            dialog_labelfieldright,
            dialog_onofffieldright,
            dialog_panel,
            dialog_root,
            dialog_selectlist,
            dialog_selectlistfield,
            dialog_textarea,

            dialog_floatimage,

            dialog_textfieldbelow,
            dialog_textfieldright,
            dialogLS_textfieldbelow_buttonright,
            dialog_multiline_labelfieldbelow,
            dialog_html,
        }

        public static View LoadFloatElementLayout(Context context, View convertView, ViewGroup parent, int layoutId, out TextView label, out SeekBar slider, out ImageView left, out ImageView right)
        {
            var layout = convertView ?? LoadLayout(context, parent, layoutId);
            if (layout != null)
            {
                label = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                slider = layout.FindViewById<SeekBar>(context.Resources.GetIdentifier("dialog_SliderField", "id", context.PackageName));
                left = layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageLeft", "id", context.PackageName));
                right = layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageRight", "id", context.PackageName));
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
                var inflater = LayoutInflater.FromContext(context);
                if (_resourceMap.ContainsKey((ElementLayout)layoutId))
                {
                    var layoutName = _resourceMap[(ElementLayout)layoutId];
                    layoutId = context.Resources.GetIdentifier(layoutName, "layout", context.PackageName);
                }
                else
                {
                    // TODO: figure out what context to use to get this right, currently doesn't inflate application resources
                    Log.Info("Android.Dialog", "LoadLayout: Failed to map resource: " + layoutId.ToString(CultureInfo.InvariantCulture));
                }

                return inflater.Inflate(layoutId, parent, false);
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
                label = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                value = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_ValueField", "id", context.PackageName));
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
                button = layout.FindViewById<Button>(context.Resources.GetIdentifier("dialog_Button", "id", context.PackageName));
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
                value = layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id", context.PackageName));
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
                label = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                value = layout.FindViewById<View>(context.Resources.GetIdentifier("dialog_BoolField", "id", context.PackageName));
                var id = context.Resources.GetIdentifier("dialog_LabelSubtextField", "id", context.PackageName);
                subLabel = (id >= 0) ? layout.FindViewById<TextView>(id) : null;
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
                label = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                value = layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id", context.PackageName));
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
                webView = layout.FindViewById<WebView>(context.Resources.GetIdentifier("dialog_HtmlField", "id", context.PackageName));
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
                label = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                value = layout.FindViewById<EditText>(context.Resources.GetIdentifier("dialog_ValueField", "id", context.PackageName));
                button = layout.FindViewById<ImageButton>(context.Resources.GetIdentifier("dialog_Button", "id", context.PackageName));
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
                achivementImage = layout.FindViewById<ImageView>(context.Resources.GetIdentifier("dialog_ImageRight", "id", context.PackageName));
                caption = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelField", "id", context.PackageName));
                description = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelSubtextField", "id", context.PackageName));
                percentageComplete = layout.FindViewById<TextView>(context.Resources.GetIdentifier("dialog_LabelPercentageField", "id", context.PackageName));
            }
            return layout;
        }

        private static readonly Dictionary<ElementLayout, string> _resourceMap;

        static DroidResources()
        {
            _resourceMap = new Dictionary<ElementLayout, string>
            {
                // Label templates
                { ElementLayout.dialog_labelfieldbelow, "dialog_labelfieldbelow"},
                { ElementLayout.dialog_labelfieldright, "dialog_labelfieldright"},

                // Boolean and Checkbox templates
                { ElementLayout.dialog_boolfieldleft, "dialog_boolfieldleft"},
                { ElementLayout.dialog_boolfieldright, "dialog_boolfieldright"},
                { ElementLayout.dialog_boolfieldsubleft, "dialog_boolfieldsubleft"},
                { ElementLayout.dialog_boolfieldsubright, "dialog_boolfieldsubright"},
                { ElementLayout.dialog_onofffieldright, "dialog_onofffieldright"},

                // Root templates
                { ElementLayout.dialog_root, "dialog_root"},

                // Entry templates
                { ElementLayout.dialog_textfieldbelow, "dialog_textfieldbelow"},
                { ElementLayout.dialog_textfieldright, "dialog_textfieldright"},
                { ElementLayout.dialogLS_textfieldbelow_buttonright, "dialog_textfieldbelow_buttonright"},

                // Slider
                { ElementLayout.dialog_floatimage, "dialog_floatimage"},

                // Button templates
                { ElementLayout.dialog_button, "dialog_button"},

                // Date
                { ElementLayout.dialog_datefield, "dialog_datefield"},

                //
                { ElementLayout.dialog_fieldsetlabel, "dialog_fieldsetlabel"},

                { ElementLayout.dialog_panel, "dialog_panel"},

                //
                { ElementLayout.dialog_selectlist, "dialog_selectlist"},
                { ElementLayout.dialog_selectlistfield, "dialog_selectlistfield"},
                { ElementLayout.dialog_textarea, "dialog_textarea"},
                { ElementLayout.dialog_multiline_labelfieldbelow, "dialog_multiline_labelfieldbelow"},
                { ElementLayout.dialog_html, "dialog_html"},
                { ElementLayout.dialog_achievements, "dialog_achievements"},
            };
        }
    }
}