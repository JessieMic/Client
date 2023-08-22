using System.ComponentModel;

namespace LogicUnit
{
    public enum eLoginErrors
    {
        [Description("Please enter "
                     + "a"
                     + " username at least two characters long. Use only letters (a-z,A-Z) and numbers.")]
        InvalidName,

        [Description("This username is already in use. Please enter a different username.")]
        NameTaken,

        [Description("Please enter a code.")]
        EmptyCode,

        Ok,

        [Description("Code not found.")]
        CodeNotFound,

        [Description("Server Error.\nPlease try again.")]
        ServerError,

        [Description("You can't join room, the room is full.")]
        FullRoom
    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum i_EnumValue)
        {
            var field = i_EnumValue.GetType().GetField(i_EnumValue.ToString());
            var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attr.Length == 0 ? i_EnumValue.ToString() : (attr[0] as DescriptionAttribute).Description;
        }
    }
}
