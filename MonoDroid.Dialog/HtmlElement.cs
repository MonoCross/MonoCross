namespace MonoDroid.Dialog
{
    public class HtmlElement : Element
    {
        public string Value;
        public HtmlElement(string caption, string value) : base(caption)
        {
            Value = value;
        }
    }
}