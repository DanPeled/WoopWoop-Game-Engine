namespace WoopWoop.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EditorWindowFor : Attribute
    {
        public Type forType;
        public EditorWindowFor(Type type)
        {
            forType = type;
        }
        
    }
}