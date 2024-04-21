// using System;
// using System.Text;
// using ZeroElectric.Vinculum;
// using ZeroElectric.Vinculum.Extensions;

// namespace WoopWoopEngine.UI
// {
//     public class TextInput : UIBoundsComponent
//     {
//         public string Value { get; set; } = "                                                "; // Property to access the value

//         public bool EditMode { get; set; } = true; // Property to toggle edit mode

//         public override void Update(float deltaTime)
//         {
//             base.Update(deltaTime);
//             // Cast string to sbyte* to pass to Raygui
//             byte[] valueBytes = Encoding.ASCII.GetBytes(Value);
//             unsafe
//             {
//                 fixed (byte* valueBytesPtr = valueBytes)
//                 {
//                     RayGui.GuiTextBox(bounds, (sbyte*)valueBytesPtr, 30, EditMode);
//                     // Update value after editing
//                     // if (!EditMode)
//                     // {
//                     Value = Encoding.ASCII.GetString(valueBytes);
//                     // }
//                 }
//             }
//         }
//     }
// }
