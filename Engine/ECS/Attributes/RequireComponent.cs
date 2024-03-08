namespace WoopWoop
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequireComponent : Attribute
    {
        public Type[] requiredComponents { get; private set; }

        public RequireComponent(params Type[] componentType)
        {
            this.requiredComponents = componentType;
        }
    }
}