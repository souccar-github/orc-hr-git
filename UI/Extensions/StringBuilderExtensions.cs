#region

using System.Text;

#endregion

namespace UI.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendLineFormat(this StringBuilder stringBuilder, string source, params object[] args)
        {
            stringBuilder.AppendFormat(source + "\r\n", args);
        }
    }
}