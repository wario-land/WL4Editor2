namespace WL4EditorCore.Exception
{
    /// <summary>
    /// An exception which is thrown when internal components of WL4Editor fail.
    /// In practice, this exception should only be thrown as part of testing or the development process.
    /// If a user experiences this exception, then something is wrong with WL4Editor.
    /// </summary>
    public class InternalException : System.Exception
    {
        public InternalException(string msg) : base(msg) { }

        public override string Message
        {
            get => $"{TargetSite}: {base.Message}";
        }
    }
}
