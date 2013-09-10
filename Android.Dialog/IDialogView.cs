namespace Android.Dialog
{
    public interface IDialogView
    {
        RootElement Root { get; }
        DialogAdapter DialogAdapter { get; }
    }

    public static class DialogViewExtensions
    {
        public static void ReloadData(this IDialogView view)
        {
            if (view.Root == null) return;
            view.DialogAdapter.ReloadData();
        }
    }
}