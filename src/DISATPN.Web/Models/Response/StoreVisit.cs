using System;
using System.Collections.Generic;

namespace DISATPN.Web.Models.Response
{
    public class StoreVisit
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<VisitedPath> Path { get; set; }
    }
}
