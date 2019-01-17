using System;

namespace DISATPN.Web.Models.Context
{
    public class Path
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid VisitId { get; set; }
    }
}
