using CallLog.LocalServer.Data;

namespace CallLog.LocalServer.Support
{
    public static class Initialiser
    {
        public static void InitialiseDatabase(DataContext context)
        {
            var c1 = new Controller { Name = "Selina Taylor", Initials = "ST" };
            var c2 = new Controller { Name = "Tony Richards", Initials = "TR" };
            var c3 = new Controller { Name = "Louise Tolson", Initials = "LT" };

            context.Controllers.AddRange(c1, c2, c3);

            context.Events.AddRange(new LoggedEvent
            {
                Mode = Data.Mode.Training,
                Name = "Bath Half Marathon",
                Controllers = new List<Controller> { c1, c2 }
            },
            new LoggedEvent
            {
                Mode = Data.Mode.Training,
                Name = "Bristol Half Marathon",
                Controllers = new List<Controller> { c2, c3 }
            },
            new LoggedEvent
            {
                Mode = Data.Mode.Operational,
                Name = "Harbour Festival",
                Controllers = new List<Controller> { c1, c2, c3 }
            },
            new LoggedEvent
            {
                Mode = Data.Mode.Operational,
                Name = "Weston Beach Race",
                Controllers = new List<Controller> { c1, c3 }
            });

            context.SaveChanges();
        }
    }
}
