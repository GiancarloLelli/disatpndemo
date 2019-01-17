using DISATPN.Web.Context;
using DISATPN.Web.Models.Context;
using DISATPN.Web.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DISATPN.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly DatabaseContext m_context;

        public VisitController(DatabaseContext ctx)
        {
            m_context = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [HttpGet]
        public ActionResult<IEnumerable<StoreVisit>> Get() => Ok(m_context.Visits.OrderByDescending(x => x.Timestamp).ToArray());

        [HttpGet("{id}")]
        public ActionResult<StoreVisit> Get(Guid id)
            => Ok(m_context.Visits.Include(x => x.Paths).OrderByDescending(p => p.Timestamp).FirstOrDefault(x => x.UserId == id));

        [HttpPost]
        public ActionResult Post(StoreVisit visitingSession)
        {
            if (ModelState.IsValid)
            {
                var storageVisit = new Visit
                {
                    SessionId = visitingSession.SessionId,
                    Timestamp = visitingSession.Timestamp,
                    UserId = visitingSession.UserId,
                    Paths = new List<Path>()
                };

                foreach (var postPath in visitingSession.Path)
                {
                    storageVisit.Paths.Add(new Path
                    {
                        DeviceId = postPath.DeviceId,
                        Timestamp = postPath.Timestamp,
                    });
                }

                m_context.Visits.Add(storageVisit);
                m_context.SaveChanges();

                return NoContent();
            }

            return BadRequest(ModelState);
        }
    }
}
