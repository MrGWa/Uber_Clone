namespace RideApp.Services
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class DriverAssigner
    {
        public static Driver AssignDriver()
        {
            // Mock driver assignment
            return new Driver { Id = 101, Name = "John Smith" };
        }
    }
}
