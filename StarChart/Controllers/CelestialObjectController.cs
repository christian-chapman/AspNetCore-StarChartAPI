using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Collections.Generic;
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
            var co = _context.CelestialObjects.Find(id);

            if (co == null) return NotFound();

            co.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == id).ToList();
            return Ok(co);
        }

        [HttpGet("{name}")]
        IActionResult GetByName(string name)
        {
            List<CelestialObject> requestedCelObjs = _context.CelestialObjects.Where(op => op.Name == name).ToList();

            if (!requestedCelObjs.Any()) return NotFound();

            foreach (CelestialObject co in requestedCelObjs)
            {
                co.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == co.Id).ToList();
            }

            return Ok(requestedCelObjs);
        }

        [HttpGet]
        IActionResult GetAll()
        {
            List<CelestialObject> allCelestialObjects = _context.CelestialObjects.ToList();

            foreach(CelestialObject co in allCelestialObjects)
            {
                co.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == co.Id).ToList();
            }

            return Ok(allCelestialObjects);
            
        }

    }
}
