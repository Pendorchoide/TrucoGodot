using Godot;

namespace TrucoProject.Net.Utils
{
    public static class NetLogger
    {
        public static bool EnableDebug = true;

        public static void Info(params object[] msg)
        {
            GD.Print("[INFO]", string.Join(" ", msg));
        }

        public static void Warn(params object[] msg)
        {
            GD.PrintRich("[color=yellow][WARN][/color] " + string.Join(" ", msg));
        }

        public static void Error(params object[] msg)
        {
            GD.PrintRich("[color=red][ERROR][/color] " + string.Join(" ", msg));
        }

        public static void Debug(params object[] msg)
        {
            if (EnableDebug)
                GD.PrintRich("[color=gray][DEBUG][/color] " + string.Join(" ", msg));
        }
    }
}
