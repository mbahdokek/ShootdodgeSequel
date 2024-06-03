using GTA;
using System.Windows.Forms;
using System.Globalization;

namespace Shootdodge
{
  public class Configs : Script
  {
    public static readonly ScriptSettings iniFile = ScriptSettings.Load("scripts\\Shootdodge.ini");
    public static Keys toggleKey;
    public static float timeScale;
    public static float hudScaleDiv;
    public static float hudPosX;
    public static float hudPosY;
    public static  float jumpForceForward;
    public static float jumpForceUpward;
    public static int updateHudMs;
    public static bool controllerEnabled;
    public static bool lowerEnemyAccuracy;
    public static bool enableHud;
    public static bool enableSfx;

    public Configs()
    {
      toggleKey = iniFile.GetValue<Keys>("Keys", "ShootdodgeKey", Keys.J, CultureInfo.InvariantCulture);
      timeScale = iniFile.GetValue<float>("Settings", "TimeScale", 0.14f, CultureInfo.InvariantCulture);
      updateHudMs = iniFile.GetValue<int>("Settings", "UpdateHudPerMs", 0, CultureInfo.InvariantCulture);
      jumpForceForward = 2.5f * iniFile.GetValue<float>("Settings", "ForwardForceMult", 2.0f, CultureInfo.InvariantCulture);
      jumpForceUpward = 0.50f * iniFile.GetValue<float>("Settings", "UpwardForceMult", 2.5f, CultureInfo.InvariantCulture);
      hudScaleDiv = iniFile.GetValue<float>("Settings", "HudScale", 2f, CultureInfo.InvariantCulture);
      hudPosX = iniFile.GetValue<float>("Settings", "HudPosX", 260f, CultureInfo.InvariantCulture);
      hudPosY = iniFile.GetValue<float>("Settings", "HudPosY", 600f, CultureInfo.InvariantCulture);
      controllerEnabled = iniFile.GetValue("Settings", "ControllerOn", true, CultureInfo.InvariantCulture);
      lowerEnemyAccuracy = iniFile.GetValue("Settings", "EnemyLessAccurate", true, CultureInfo.InvariantCulture);
      enableHud = iniFile.GetValue("Settings", "EnableHud", true, CultureInfo.InvariantCulture);
      enableSfx = iniFile.GetValue("Settings", "EnableSfx", true, CultureInfo.InvariantCulture);
    }
  }
}
