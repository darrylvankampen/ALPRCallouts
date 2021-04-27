using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ALPRCallouts
{
    public class Utils
    {
        internal static readonly Random rnd = new Random();
        internal static Vector3 GetRandomPosition(int min = 200, int max = 750)
        {
            int distance = rnd.Next(min, max);
            float offsetX = rnd.Next(-1 * distance, distance);
            float offsetY = rnd.Next(-1 * distance, distance);
            return new Vector3(offsetX, offsetY, 0);
        }
        
        internal static int GetRandomNumber()
        {
            return rnd.Next(1, 100 + 1);
        }

        internal static int GetRandomNumberBetween(int numberOne, int numberTwo)
        {
            return rnd.Next(numberOne, numberTwo);
        }
        
        internal static void Notify(string message)
        {
            API.BeginTextCommandThefeedPost("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandThefeedPostTicker(false,true);
        }

        internal static WeaponHash GetRandomWeapon()
        {
            List<WeaponHash> weps = new List<WeaponHash>
            {
                WeaponHash.Pistol,
                WeaponHash.Pistol50,
                WeaponHash.PistolMk2,
                WeaponHash.CombatPistol,
                WeaponHash.HeavyPistol,
                WeaponHash.MachinePistol,
                WeaponHash.MarksmanPistol,
                WeaponHash.VintagePistol,
                WeaponHash.APPistol,
                WeaponHash.SNSPistol,
                WeaponHash.SNSPistolMk2,
                WeaponHash.Revolver,
                WeaponHash.RevolverMk2
            };
            return weps[GetRandomNumberBetween(0, weps.Count)];
        }
        
        internal static VehicleHash GetRandomVehicle()
        {
            List <VehicleHash> vehs = new List<VehicleHash>
            {
                VehicleHash.Adder,
                VehicleHash.Airbus,
                VehicleHash.Akuma
            };
            return vehs[GetRandomNumberBetween(0, vehs.Count)];
        }
    }
}