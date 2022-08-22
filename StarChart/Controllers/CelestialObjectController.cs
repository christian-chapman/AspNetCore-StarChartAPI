using System.Linq;
using System.Net;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var Co = _context.CelestialObjects.Find(id);
            if (Co == null)
                return NotFound();
            Co.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == id).ToList();
            return Ok(Co);

        }

        [HttpGet("{name}")]
        IActionResult GetByName(string name)
        {
            CelestialObject requestedCelObj = _context.CelestialObjects.Find(name);

            if (requestedCelObj == null) return NotFound();

            foreach (CelestialObject obj in _context.CelestialObjects)
            {
                if (obj.OrbitedObjectId == requestedCelObj.Id)
                {
                    requestedCelObj.Satellites.Add(obj);
                }
            }

            return Ok(requestedCelObj);

        }

        [HttpGet]
        IActionResult GetAll()
        {
            
            return Ok();
            
        }

    }
}
