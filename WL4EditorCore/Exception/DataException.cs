namespace WL4EditorCore.Exception
{
    /// <summary>
    /// An exception which is thrown when WL4Editor encounters an unrecoverable data-level error.
    /// </summary>
    public class DataException : System.Exception
    {
        public DataException(string msg) : base(msg) { }

        public override string Message
        {
            get => $"{TargetSite}: {base.Message}";
        }
    }
}