namespace WoopWoopEngine
{
    /// <summary>
    /// Specifies that a component requires certain other components to be attached to the same entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequireComponent : Attribute
    {
        /// <summary>
        /// Gets the array of required component types.
        /// </summary>
        public Type[] RequiredComponents { get; private set; }

        /// <summary>
        /// Gets the message type associated with the requirement.
        /// </summary>
        public MessageType MessageType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireComponent"/> class with the specified message type and component types.
        /// </summary>
        /// <param name="type">The message type associated with the requirement.</param>
        /// <param name="componentTypes">The array of component types required by the attributed component.</param>
        public RequireComponent(MessageType type, params Type[] componentTypes)
        {
            MessageType = type;
            RequiredComponents = componentTypes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireComponent"/> class with the specified component types and defaults to an error message type.
        /// </summary>
        /// <param name="componentTypes">The array of component types required by the attributed component.</param>
        public RequireComponent(params Type[] componentTypes)
        {
            MessageType = MessageType.Error;
            RequiredComponents = componentTypes;
        }
    }

    /// <summary>
    /// Specifies the type of message associated with a requirement.
    /// </summary>
    public enum MessageType
    {
        Log, Warning, Error
    }
}
