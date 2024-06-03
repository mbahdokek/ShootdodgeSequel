using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System;

namespace Shootdodge
{
    public static class Utils
    {
        private static readonly Dictionary<uint, string> weapHashToGroup;

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
    }
}
