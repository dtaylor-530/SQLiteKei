namespace Utility.Common.Base;

public interface IMessageBoxService
{
    public bool? ShowMessage(MessageBoxConfiguration configuration);

}

//
// Summary:
//     Specifies the buttons that are displayed on a message box. Used as an argument
//     of the Overload:System.Windows.MessageBox.Show method.
public enum MessageBoxButton
{
    //
    // Summary:
    //     The message box displays an OK button.
    OK = 0,
    //
    // Summary:
    //     The message box displays OK and Cancel buttons.
    OKCancel = 1,
    //
    // Summary:
    //     The message box displays Yes, No, and Cancel buttons.
    YesNoCancel = 3,
    //
    // Summary:
    //     The message box displays Yes and No buttons.
    YesNo = 4
}

//
// Summary:
//     Specifies the icon that is displayed by a message box.
public enum MessageBoxImage
{
    //
    // Summary:
    //     The message box contains no symbols.
    None = 0,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Error = 0x10,
    //
    // Summary:
    //     The message box contains a symbol consisting of a white X in a circle with a
    //     red background.
    Hand = 0x10,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Stop = 0x10,
    //
    // Summary:
    //     The message box contains a symbol consisting of a question mark in a circle.
    //     The question mark message icon is no longer recommended because it does not clearly
    //     represent a specific type of message and because the phrasing of a message as
    //     a question could apply to any message type. In addition, users can confuse the
    //     question mark symbol with a help information symbol. Therefore, do not use this
    //     question mark symbol in your message boxes. The system continues to support its
    //     inclusion only for backward compatibility.
    Question = 0x20,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Exclamation = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Warning = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Asterisk = 0x40,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Information = 0x40
}

public record MessageBoxConfiguration(string Body, string Title, MessageBoxButton MessageBoxButton, MessageBoxImage MessageBoxImage);
