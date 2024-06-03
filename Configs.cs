using GTA;
using System.Windows.Forms;
using System.Globalization;

namespace Shootdodge
{
  public class Configs : Script
  {
    public static readonly ScriptSettings iniFile = ScriptSettings.Load("scripts\\Shootdodge.ini");
    public static Keys key;
    public static float timeScale;
    public static float hudScaleDiv;
    public static float hudPosX;
    public static float hudPosY;
    public static float forceFwd;
    public static float forceUp;
    public static bool controllerOn;
    public static bool nerfAcc;
    public static bool enableHud;
    public static bool enableSfx;

    public Configs()
    {
      key = iniFile.GetValue<Keys>("Keys", "ShootdodgeKey", Keys.J, CultureInfo.InvariantCulture);
      timeScale = iniFile.GetValue<float>("Settings", "TimeScale", 0.14f, CultureInfo.InvariantCulture);
      forceFwd = iniFile.GetValue<float>("Settings", "ForwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
      forceUp = iniFile.GetValue<float>("Settings", "UpwardForce", 10.0f, CultureInfo.InvariantCulture) * 100f;
      hudScaleDiv = iniFile.GetValue<float>("Settings", "HudScale", 2f, CultureInfo.InvariantCulture);
      hudPosX = iniFile.GetValue<float>("Settings", "HudPosX", 260f, CultureInfo.InvariantCulture);
      hudPosY = iniFile.GetValue<float>("Settings", "HudPosY", 600f, CultureInfo.InvariantCulture);
      controllerOn = iniFile.GetValue("Settings", "OnController", true, CultureInfo.InvariantCulture);
      nerfAcc = iniFile.GetValue("Settings", "EnemyLessAccurate", true, CultureInfo.InvariantCulture);
      enableHud = iniFile.GetValue("Settings", "EnableHud", true, CultureInfo.InvariantCulture);
      enableSfx = iniFile.GetValue("Settings", "EnableSfx", true, CultureInfo.InvariantCulture);
    }
  }
}
