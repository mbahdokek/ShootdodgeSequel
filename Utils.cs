using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace Shootdodge
{
    public static class Utils
    {
        private static readonly Dictionary<uint, string> weapHashToGroup;
        private static DateTime timer = DateTime.Now;

        static Utils()
        {
            weapHashToGroup = new Dictionary<uint, string>();
            foreach (WeaponGroup weapon in Enum.GetValues(typeof(WeaponGroup)))
            {
                weapHashToGroup[(uint)weapon] = weapon.ToString();
            }
        }

        public static string GetWeaponGroupFromHash(uint hash)
        {
            if (weapHashToGroup.TryGetValue(hash, out string name))
            {
                return name;
            }
            return null;
        }

        public static void FacePosition(this Ped ped, Vector3 pos)
        {
            ped.Heading = Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, (pos.X - ped.Position.X), (pos.Y - ped.Position.Y));
        }

        public static void FakeShootRate(Ped ped)
        {
            float delay = Function.Call<float>(Hash.GET_WEAPON_TIME_BETWEEN_SHOTS, ped.Weapons.Current.Hash) * 1000f;
            if ((DateTime.Now - timer).TotalMilliseconds >= delay * 2f)
            {
                Weapon pedWpn = ped.Weapons.Current;
                if (pedWpn.AmmoInClip != 0)
                World.ShootSingleBullet(ped.Bones[Bone.SkelRightHand].Position, World.GetCrosshairCoordinates().HitPosition, (int)Function.Call<float>(Hash.GET_WEAPON_DAMAGE, pedWpn.Hash) / 2, pedWpn.Hash, ped, false);
                pedWpn.AmmoInClip -= 1;
                timer = DateTime.Now;
            }
        }

        public static void PlayerDamage(float damage)
        {
            Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, damage);
        }

        public static void CheckDualWield()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Main.DualWield = assembly.GetType("DualWield.Main");
                if (Main.DualWield != null)
                {
                    Main.DualWieldField = Main.DualWield.GetField("DualWielding");
                    break;
                }
            }
        }

        public static bool DualWielding()
        {
            if (Main.DualWieldField != null)
                return (bool)Main.DualWieldField.GetValue(null);
            else
                return false;
        }
    }
}