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

    }
}
