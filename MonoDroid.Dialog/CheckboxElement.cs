using Android.Content;
using Android.Views;

namespace MonoDroid.Dialog
{
    public class CheckboxElement : Element
    {
        private readonly Context _context;

        public new bool Value;
        public string Group;

        public CheckboxElement(Context context, string caption)
            : base(caption)
        {
            _context = context;
        }

        public CheckboxElement(Context context, string caption, bool value)
            : this(context, caption)
        {
            Value = value;
        }

        public CheckboxElement(Context context, string caption, bool value, string group)
            : this(context, caption, value)
        {
            Group = group;
        }

        View ConfigCell(View cell)
        {
            //cell.Accessory = Value ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
            return cell;
        }

        public override View GetView()
        {
            return ConfigCell(base.GetView());
        }

        public override void Selected()
        {
            //Value = !Value;
            //var cell = tableView.CellAt(path);
            //ConfigCell(cell);
            //base.Selected(dvc, tableView, path);
            base.Selected();
        }
    }
}