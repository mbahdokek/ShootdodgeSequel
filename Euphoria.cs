using GTA;
using GTA.Native;

public static class Euphoria
{
  public static void SendNMMessage(this Ped ped, NMMessage id, int duration = -1)
  {
    ped.CanRagdoll = true;
    Function.Call(Hash.SET_PED_TO_RAGDOLL, ped, duration, duration, 1, true, true, false);
    Function.Call(Hash.CREATE_NM_MESSAGE, true,(int) id);
    Function.Call(Hash.GIVE_PED_NM_MESSAGE, ped);
  }

  public enum NMMessage
  {
    stopAllBehaviours = 0,
    armsWindmill = 372, // 0x00000174
    bodyBalance = 466, // 0x000001D2
    bodyFoetal = 507, // 0x000001FB
    bodyWrithe = 526, // 0x0000020E
    braceForImpact = 548, // 0x00000224
    catchFall = 576, // 0x00000240
    highFall = 715, // 0x000002CB
    injuredOnGround = 787, // 0x00000313
    pedalLegs = 816, // 0x00000330
    rollDownStairs = 941, // 0x000003AD
    staggerFall = 1151, // 0x0000047F
    teeter = 1221, // 0x000004C5
    yanked = 1249, // 0x000004E1
  }
}
