using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ALPRCallouts
{
    public class Config : BaseScript
    {
        public static int hasPassenger, chanceOfStartingPursuit, passengerHavingWeapon;
        public static List<String> Warrants;

        public static void LoadConf()
        {
            var File = API.LoadResourceFile("fivepd", "callouts/ALPRCallouts/callout-settings.json");
            dynamic config = JsonConvert.DeserializeObject(File);

            try
            {
                if (config != null)
                {
                    hasPassenger = (int) config["global"]["hasPassenger"];
                    chanceOfStartingPursuit = (int) config["global"]["chanceOfStartingPursuit"];
                    passengerHavingWeapon = (int) config["global"]["passengerHavingWeapon"];

                    JArray WarrantArray = (JArray) config["warrants"];
                    Warrants = WarrantArray.ToObject<List<String>>();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ALPRCallouts: " + e);
                hasPassenger = 30;
                chanceOfStartingPursuit = 45;
                passengerHavingWeapon = 25;
                Warrants = new List<string>
                {
                    "NO DRIVERS LICENSE",
                    "PUBLIC INTOXICATION",
                    "CRIMINAL THREATENING",
                    "SECOND DEGREE ASSAULT",
                    "SHOPLIFTING",
                    "STALKING",
                    "BURGLARY",
                    "SALE OF A CONTROLLED DRUG",
                    "RECEIVING / POSSESSION OF STOLEN PROPERTY",
                    "FELONIOUS USE OF A FIREARM",
                    "SIMPLE ASSAULT",
                    "CRIMINAL TRESPASS",
                    "THEFT",
                    "VIOL OF BAIL CONDITIONS",
                    "ORGANISED CRIME",
                    "THEFT OF A MOTOR VEHICLE",
                    "POSSESSION OF DANGEROUS WEAPON",
                    "POSSESSION OF DRUGS",
                    "CONDUCT AFTER ACCIDENT",
                    "CRIMINAL MISCHIEF",
                    "SEX OFFENDER FAIL TO REGISTER",
                    "CONTEMPT OF COURT",
                    "DUTY TO REPORT",
                    "BREACH OF BAIL",
                    "OBSTRUCTING",
                    "VIOLATION OF PROTECTIVE ORDER"
                };
            }
        }
    }
}