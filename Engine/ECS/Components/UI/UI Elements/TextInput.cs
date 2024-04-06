// using System.Text;
// using ZeroElectric.Vinculum;
// using ZeroElectric.Vinculum.Extensions;

// namespace WoopWoop.UI
// {
//     public class TextInput : Renderer
//     {
//         public string value = "yes";
//         public bool editMode = true;

//         Rectangle bounds;

//         public override void Update(float deltaTime)
//         {
//             bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);
//             unsafe
//             {
//                 string title = "hello", message = "yee", buttons = " ";
//                 byte[] valueBytes = Encoding.ASCII.GetBytes(value);
//                 byte[] titleBytes = Encoding.ASCII.GetBytes(title);
//                 byte[] messageBytes = Encoding.ASCII.GetBytes(message);
//                 byte[] buttonsBytes = Encoding.ASCII.GetBytes(buttons);
//                 fixed (byte* valueBytesPtr = valueBytes, titleBytesPtr = titleBytes, buttonBytesPtr = buttonsBytes, messageBytesPtr = buttonsBytes)
//                 {
//                     Bool b = new Bool(true);
//                     RayGui.GuiTextInputBox(bounds, (sbyte*)titleBytesPtr, (sbyte*)messageBytesPtr, (sbyte*)buttonBytesPtr, (sbyte*)valueBytesPtr, 20, &b);
//                 }
//             }
//         }
//     }
// }
//TODO: figure out why the input didnt work