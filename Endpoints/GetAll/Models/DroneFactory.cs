namespace ociusApi.Models
{
    public class DroneFactory
    {
        public static IDrone GetDroneType(string droneType)
        {
            return droneType.Contains("Location") 
                ? (IDrone)new DroneLocation() 
                : (IDrone)new DroneSensor();
        }
    }
}
