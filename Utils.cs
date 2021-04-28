using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

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
                VehicleHash.Alpha,
                VehicleHash.Banshee,
                VehicleHash.Banshee2,
                VehicleHash.Blista,
                VehicleHash.Blista2,
                VehicleHash.Blista3,
                VehicleHash.Buffalo,
                VehicleHash.Buffalo2,
                VehicleHash.Buffalo3,
                VehicleHash.Carbonizzare,
                VehicleHash.Comet2,
                VehicleHash.Comet3,
                VehicleHash.Coquette,
                VehicleHash.Coquette2,
                VehicleHash.Coquette3,
                VehicleHash.Elegy,
                VehicleHash.Furoregt,
                VehicleHash.Fusilade,
                VehicleHash.Futo,
                VehicleHash.Jester,
                VehicleHash.Massacro,
                VehicleHash.Penumbra,
                VehicleHash.RapidGT,
                VehicleHash.Schafter2,
                VehicleHash.Schwarzer,
                VehicleHash.Sultan,
                VehicleHash.SultanRS,
                VehicleHash.Surano,
                VehicleHash.Blade,
                VehicleHash.Buccaneer,
                VehicleHash.Chino,
                VehicleHash.Dominator,
                VehicleHash.Dukes,
                VehicleHash.Gauntlet,
                VehicleHash.Hotknife,
                VehicleHash.Phoenix,
                VehicleHash.Picador,
                VehicleHash.RatLoader,
                VehicleHash.Ruiner,
                VehicleHash.SabreGT,
                VehicleHash.SabreGT2,
                VehicleHash.SlamVan,
                VehicleHash.Vigero,
                VehicleHash.Virgo,
                VehicleHash.Voodoo,
                VehicleHash.Adder,
                VehicleHash.Bullet,
                VehicleHash.Cheetah,
                VehicleHash.EntityXF,
                VehicleHash.Infernus,
                VehicleHash.Osiris,
                VehicleHash.T20,
                VehicleHash.Turismor,
                VehicleHash.Vacca,
                VehicleHash.Voltic,
                VehicleHash.Zentorno,
                VehicleHash.JB700,
                VehicleHash.Manana,
                VehicleHash.Monroe,
                VehicleHash.Peyote,
                VehicleHash.Pigalle,
                VehicleHash.Stinger,
                VehicleHash.StingerGT,
                VehicleHash.Tornado,
                VehicleHash.ZType,
                VehicleHash.Asea,
                VehicleHash.Asterope,
                VehicleHash.Emperor,
                VehicleHash.Fugitive,
                VehicleHash.Glendale,
                VehicleHash.Ingot,
                VehicleHash.Intruder,
                VehicleHash.Premier,
                VehicleHash.Primo,
                VehicleHash.Regina,
                VehicleHash.Romero,
                VehicleHash.Schafter2,
                VehicleHash.Stanier,
                VehicleHash.Stratum,
                VehicleHash.Superd,
                VehicleHash.Surge,
                VehicleHash.Tailgater,
                VehicleHash.Warrener,
                VehicleHash.Washington,
                VehicleHash.Baller,
                VehicleHash.BJXL,
                VehicleHash.Cavalcade,
                VehicleHash.Cavalcade2,
                VehicleHash.Dubsta,
                VehicleHash.Dubsta2,
                VehicleHash.FQ2,
                VehicleHash.Granger,
                VehicleHash.Gresley,
                VehicleHash.Habanero,
                VehicleHash.Huntley,
                VehicleHash.Landstalker,
                VehicleHash.Mesa,
                VehicleHash.Patriot,
                VehicleHash.Radi,
                VehicleHash.Rocoto,
                VehicleHash.Seminole,
                VehicleHash.Serrano,
                VehicleHash.Blista,
                VehicleHash.Dilettante,
                VehicleHash.Issi2,
                VehicleHash.Panto,
                VehicleHash.Prairie,
                VehicleHash.Rhapsody,
                VehicleHash.Exemplar,
                VehicleHash.F620,
                VehicleHash.Felon,
                VehicleHash.Jackal,
                VehicleHash.Oracle,
                VehicleHash.Oracle2,
                VehicleHash.Sentinel,
                VehicleHash.Windsor,
                VehicleHash.Zion,
                VehicleHash.Zion2,
                VehicleHash.Bifta,
                VehicleHash.Blazer,
                VehicleHash.Bodhi2,
                VehicleHash.Dubsta,
                VehicleHash.Dune,
                VehicleHash.Dune2,
                VehicleHash.Dune3,
                VehicleHash.Dune4,
                VehicleHash.Dune5,
                VehicleHash.BfInjection,
                VehicleHash.Kalahari,
                VehicleHash.Marshall,
                VehicleHash.RancherXL,
                VehicleHash.Rebel,
                VehicleHash.Rebel2,
                VehicleHash.Sandking,
                VehicleHash.Sandking2,
                VehicleHash.Bison,
                VehicleHash.BobcatXL,
                VehicleHash.Boxville,
                VehicleHash.Burrito,
                VehicleHash.Camper,
                VehicleHash.Journey,
                VehicleHash.Minivan,
                VehicleHash.Minivan2,
                VehicleHash.Pony,
                VehicleHash.Rumpo,
                VehicleHash.Speedo,
                VehicleHash.Surfer,
                VehicleHash.Surfer2,
                VehicleHash.Taco,
                VehicleHash.Youga
            };
            return vehs[GetRandomNumberBetween(0, vehs.Count)];
        }
    }
}