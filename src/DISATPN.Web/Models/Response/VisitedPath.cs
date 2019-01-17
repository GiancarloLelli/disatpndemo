using System;

namespace DISATPN.Web.Models.Response
{
    public class VisitedPath
    {
        public Guid SessionId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
