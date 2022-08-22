using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        public IActionResult GetByName(string name)
        {
            var Co = _context.CelestialObjects.Where(op => op.Name == name).ToList();
            if (!Co.Any())
                return NotFound();
            foreach (var obj in Co)
            {
                obj.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(Co);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Co = _context.CelestialObjects.ToList();

            foreach (var obj in Co)
            {
                obj.Satellites = _context.CelestialObjects.Where(op => op.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(Co);

        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new {id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            CelestialObject co = _context.CelestialObjects.Find(id);

            if (co == null) return NotFound();

            co.Name = celestialObject.Name;
            co.OrbitalPeriod = celestialObject.OrbitalPeriod;
            co.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.Update(co);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            CelestialObject co = _context.CelestialObjects.Find(id);

            if (co == null) return NotFound();

            co.Name = name;

            _context.Update(co);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(op => (op.Id == id) || (op.OrbitedObjectId == id)).ToList();

            if (!celestialObjects.Any()) return NotFound();

            _context.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
