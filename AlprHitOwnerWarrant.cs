using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace ALPRCallouts
{
    [CalloutProperties("ALPR Hit (Warrant)", "Darihon", "1.0")]
    public class AlprHitOwnerWarrant : Callout
    {
        private Ped Suspect, Passenger;
        private PedData SuspectData;
        private String SuspectWarrant;

        private Vehicle Vehicle;
        private VehicleData VehicleData;

        private Blip Blip;
        
        public AlprHitOwnerWarrant()
        {
            InitInfo(World.GetNextPositionOnStreet(
                Game.PlayerPed.GetOffsetPosition(Utils.GetRandomPosition(300, 800))));
            ShortName = "ALPR Hit Owner Warrant (" + GenerateRandomWarrant() + ")";
            CalloutDescription = "We have received an ALPR hit on a vehicle. Vehicle owner has a warrant for " + this.SuspectWarrant;
            ResponseCode = 2;
            StartDistance = 150f;
        }

        public async override Task OnAccept()
        {
            CreateBlip();
            UpdateData();
            Utils.Notify("Vehicle information will be forwarded as soon as possible.");
        }

        public override async void OnStart(Ped player)
        {
            base.OnStart(player);

            this.Suspect = await SpawnPed(RandomUtils.GetRandomPed(), Location + 1);
            this.Vehicle = await SpawnVehicle(Utils.GetRandomVehicle(), Location);
            this.VehicleData = await Utilities.GetVehicleData(this.Vehicle.NetworkId);
            this.Suspect.SetIntoVehicle(this.Vehicle, VehicleSeat.Driver);
            this.Suspect.AlwaysKeepTask = true;
            this.Suspect.BlockPermanentEvents = true;
            this.SuspectData = await this.Suspect.GetData();
            this.SuspectData.Warrant = SuspectWarrant;
            var violation = new Violation();
            violation.Offence = "<" + SuspectWarrant + ">";
            violation.Charge = "<" + GenerateRandomCharge() + ">";
            this.SuspectData.Violations.Add(violation);
            this.Suspect.SetData(this.SuspectData);

            int passengerChance = Utils.GetRandomNumber();
            if (passengerChance < 50)
            {
                this.Passenger = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
                this.Passenger.SetIntoVehicle(this.Vehicle, VehicleSeat.Passenger);
                this.Passenger.AlwaysKeepTask = true;
                this.Passenger.BlockPermanentEvents = true;
            }
            this.Vehicle.AttachBlip();

            int randomChance = Utils.GetRandomNumber();
            if (randomChance >= 65)
            {
                Utilities.ExcludeVehicleFromTrafficStop(this.Vehicle.NetworkId, true);
                Utils.Notify("Suspect(s) are fleeing in a " + this.VehicleData.Color + " " +  this.VehicleData.Name);
                Utils.Notify("License plate: " + this.VehicleData.LicensePlate);
                API.SetDriveTaskMaxCruiseSpeed(this.Suspect.GetHashCode(), 40f);
                API.SetDriveTaskDrivingStyle(this.Suspect.GetHashCode(), 786468);
                API.SetDriverAbility(this.Suspect.GetHashCode(), 1.0f);
                API.SetDriverAggressiveness(this.Suspect.GetHashCode(), 1.0f);
                API.SetDriverRacingModifier(this.Suspect.GetHashCode(), 1.0f);
                this.Suspect.Task.FleeFrom(player);
                Pursuit.RegisterPursuit(this.Suspect);
                int randomChanceOfShootingPassenger = Utils.GetRandomNumber();
                if (randomChanceOfShootingPassenger <= 35)
                {
                    this.Passenger.Weapons.Give(Utils.GetRandomWeapon(), 1000, true, true);
                    this.Passenger.Task.FightAgainst(player);
                }
                Blip.Delete();
            }
            else
            {
                this.Suspect.Task.CruiseWithVehicle(this.Vehicle, 25f, 786603);
                Utils.Notify("Vehicle information: \n" + this.VehicleData.Color + " " + this.VehicleData.Name);
                Utils.Notify("License plate: " + this.VehicleData.LicensePlate);
            }
        }

        private String GenerateRandomWarrant()
        {
            List<String> warrants = new List<string>
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
            this.SuspectWarrant = warrants[Utils.GetRandomNumberBetween(0, warrants.Count)];
            return this.SuspectWarrant;
        }

        private String GenerateRandomCharge()
        {
            List<String> charges = new List<string>
            {
                "$100 FINE",
                "$200 FINE",
                "$300 FINE",
                "$400 FINE",
                "$500 FINE",
                "1 MO PRISON",
                "2 MO PRISON",
                "3 MO PRISON",
                "4 MO PRISON",
                "5 MO PRISON",
                "6 MO PRISON",
                "7 MO PRISON",
                "8 MO PRISON",
                "9 MO PRISON",
                "10 MO PRISON",
                "11 MO PRISON",
                "1 YEAR PRISON",
                "2 YEARS PRISON",
                "3 YEARS PRISON",
                "4 YEARS PRISON",
                "5 YEARS PRISON",
                "6 YEARS PRISON",
                "7 YEARS PRISON",
                "8 YEARS PRISON",
                "9 YEARS PRISON",
                "10 YEARS PRISON",
            };
            return charges[Utils.GetRandomNumberBetween(0, charges.Count)];
        } 

        private void CreateBlip(float circleRadius = 75f, BlipColor color = BlipColor.Red, BlipSprite sprite = BlipSprite.BigCircle, int alpha = 100)
        {
            Blip = World.CreateBlip(this.Location, circleRadius);
            this.Radius = circleRadius;
            this.Marker = Blip;
            this.Marker.Sprite = sprite;
            this.Marker.Color = color;
            this.Marker.Alpha = alpha;
        }
    }
}