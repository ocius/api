namespace ociusApi.Models
{
    public class DroneFactory
    {
        public static IDrone CreateDrone(string droneType)
        {
            return droneType.Contains("Location") 
                ? (IDrone)new DroneLocation() 
                : (IDrone)new DroneSensor();
        }
    }
}
