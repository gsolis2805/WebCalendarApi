namespace Web_Calendar_Api.Models
{
    public class Event
    {
        public Event()
        {
            this.Start = new EventDateTime()
            {
                TimeZone = "America/Guayaquil"
            };
            this.End = new EventDateTime()
            {
                TimeZone = "America/Guayaquil"
            };
        }

        public string Id { get; set; }
        public string Summary { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
        public string Description { get; set; }

    }
    public class EventDateTime
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }

    }
}
