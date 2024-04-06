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
            messageType = type;
            requiredComponents = componentTypes;
        }
        public RequireComponent(params Type[] componentTypes)
        {
            messageType = MessageType.Error;
            requiredComponents = componentTypes;
        }
    }
    public enum MessageType
    {
        Log, Warning, Error
    }
}