using System;
using System.Collections.Generic;

namespace DISATPN.Web.Models.Context
{
    public class Visit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime Timestamp { get; set; }
        public IList<Path> Paths { get; set; }
    }
}
