using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace efdemo.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var users = _context.Users.ToList();
            return users;
        }

        // GET: api/<controller>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }

        // POST: api/<controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
