using System;

namespace AgendaProject.Models
{
    public partial class MeetingTime
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime CutoffDate { get; set; }
        public string AgendaUrl { get; set; }
        public string MinutesUrl { get; set; }
        public string RecordingUrl { get; set; }
}

    public partial class MeetingTime
    {
        // Navigation Properties
    }
}