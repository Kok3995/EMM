namespace EMM.Core
{
    public enum MessageImage
    {        //
        // Summary:
        //     No icon is displayed.
        None = 0,
        //
        // Summary:
        //     The message box contains a symbol consisting of a white X in a circle with a
        //     red background.
        Hand = 0x10,
        //
        // Summary:
        //     The message box contains a symbol consisting of a question mark in a circle.
        Question = 0x20,
        //
        // Summary:
        //     The message box contains a symbol consisting of an exclamation point in a triangle
        //     with a yellow background.
        Exclamation = 48,
        //
        // Summary:
        //     The message box contains a symbol consisting of a lowercase letter i in a circle.
        Asterisk = 0x40,
        //
        // Summary:
        //     The message box contains a symbol consisting of white X in a circle with a red
        //     background.
        Stop = 0x10,
        //
        // Summary:
        //     The message box contains a symbol consisting of white X in a circle with a red
        //     background.
        Error = 0x10,
        //
        // Summary:
        //     The message box contains a symbol consisting of an exclamation point in a triangle
        //     with a yellow background.
        Warning = 48,
        //
        // Summary:
        //     The message box contains a symbol consisting of a lowercase letter i in a circle.
        Information = 0x40
    }
}
