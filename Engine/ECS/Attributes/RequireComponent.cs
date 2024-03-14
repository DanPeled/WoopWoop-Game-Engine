using System.Net.WebSockets;

namespace WoopWoop
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequireComponent : Attribute
    {
        public Type[] requiredComponents { get; private set; }

        public MessageType messageType;
        public RequireComponent(MessageType type, params Type[] componentTypes)
        {
            this.messageType = type;
            this.requiredComponents = componentTypes;
        }
        public RequireComponent(params Type[] componentTypes)
        {
            this.messageType = MessageType.Error;
            this.requiredComponents = componentTypes;
        }
    }
    public enum MessageType
    {
        Log, Warning, Error
    }
}