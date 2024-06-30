using GTA;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Shootdodge
{
    public class Configs : Script
    {
        private static string GetPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path) + "\\Shootdodge\\";
        }
        public static readonly ScriptSettings iniFile = ScriptSettings.Load(Path.Combine(GetPath(), "Shootdodge.ini"));
        public static readonly bool IniFound = File.Exists(Path.Combine(GetPath(), "Shootdodge.ini"));
        public static readonly string texturePath = GetPath();
        public static readonly Keys key = iniFile.GetValue("Keys", "ShootdodgeKey", Keys.J, CultureInfo.InvariantCulture);
        public static readonly bool controllerOn = iniFile.GetValue("Keys", "OnController", true, CultureInfo.InvariantCulture);
        public static readonly float timeScale = iniFile.GetValue("Dodging", "TimeScale", 0.14f, CultureInfo.InvariantCulture);
        public static readonly float xyForce = iniFile.GetValue("Dodging", "ForwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
        public static readonly float zForce = iniFile.GetValue("Dodging", "UpwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
        public static readonly float hudScaleDiv = iniFile.GetValue("HUD", "HudScale", 2.80f, CultureInfo.InvariantCulture);
        public static readonly float hudPosX = iniFile.GetValue("HUD", "HudPosX", 220f, CultureInfo.InvariantCulture);
        public static readonly float hudPosY = iniFile.GetValue("HUD", "HudPosY", 615f, CultureInfo.InvariantCulture);
        public static bool enableHud = iniFile.GetValue("HUD", "EnableHud", true, CultureInfo.InvariantCulture); 
        public static readonly bool nerfAcc = iniFile.GetValue("Misc", "EnemyLessAccurate", true, CultureInfo.InvariantCulture);
        public static readonly bool enableSfx = iniFile.GetValue("Misc", "EnableSfx", true, CultureInfo.InvariantCulture);
        public static readonly bool wideCam = iniFile.GetValue("Misc", "WiderCam", false, CultureInfo.InvariantCulture);
        public static readonly bool shootFast = iniFile.GetValue("Misc", "FasterShootRate", true, CultureInfo.InvariantCulture);
    }
}