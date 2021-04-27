using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace ALPRCallouts
{
    [CalloutProperties("ALPR Hit (Owner License Suspended)", "Darihon", "1.0")]
    public class AlprHitOwnerLicenseSuspended : Callout
    {
        private Ped Suspect, Passenger;
        private PedData SuspectData;

        private Vehicle Vehicle;
        private VehicleData VehicleData;

        private Blip Blip;
        
        public AlprHitOwnerLicenseSuspended()
        {
            InitInfo(World.GetNextPositionOnStreet(
                Game.PlayerPed.GetOffsetPosition(Utils.GetRandomPosition(300, 800))));
            ShortName = "ALPR Hit (Owner License Suspended)";
            CalloutDescription = "We have received an ALPR hit on a vehicle. Vehicle owner has a suspended license.";
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
            this.SuspectData.DriverLicense.LicenseStatus = PedData.License.Status.Suspended;
            Utilities.SetPedData(this.Suspect.NetworkId, this.SuspectData);

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
                    this.Passenger.Weapons.Give(WeaponHash.Pistol50, 1000, true, true);
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