using GTA;
using System.Globalization;
using System.Windows.Forms;

namespace Shootdodge
{
    public class Configs : Script
    {
        public static readonly ScriptSettings iniFile = ScriptSettings.Load("scripts\\Shootdodge.ini");
        public static readonly Keys key = iniFile.GetValue<Keys>("Keys", "ShootdodgeKey", Keys.J, CultureInfo.InvariantCulture);
        public static readonly float timeScale = iniFile.GetValue<float>("Settings", "TimeScale", 0.14f, CultureInfo.InvariantCulture);
        public static readonly float hudScaleDiv = iniFile.GetValue<float>("Settings", "HudScale", 2f, CultureInfo.InvariantCulture);
        public static readonly float hudPosX = iniFile.GetValue<float>("Settings", "HudPosX", 260f, CultureInfo.InvariantCulture);
        public static readonly float hudPosY = iniFile.GetValue<float>("Settings", "HudPosY", 600f, CultureInfo.InvariantCulture);
        public static readonly float xyForce = iniFile.GetValue<float>("Settings", "ForwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
        public static readonly float zForce = iniFile.GetValue<float>("Settings", "UpwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
        public static readonly bool controllerOn = iniFile.GetValue("Settings", "OnController", true, CultureInfo.InvariantCulture);
        public static readonly bool nerfAcc = iniFile.GetValue("Settings", "EnemyLessAccurate", true, CultureInfo.InvariantCulture);
        public static readonly bool enableHud = iniFile.GetValue("Settings", "EnableHud", true, CultureInfo.InvariantCulture);
        public static readonly bool enableSfx = iniFile.GetValue("Settings", "EnableSfx", true, CultureInfo.InvariantCulture);
    }
}
