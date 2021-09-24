using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository;

namespace efdemo.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            // Было ранее. Без использования Unit of Work.
            // var users = _context.Users.ToList();
            // return users;

            var users = _unitOfWork.Users.GetAll().ToList();
            return users;
        }

        // GET: api/<controller>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            // Было ранее. Без использования Unit of Work.
            // var user = _context.Users.Find(id);
            // return user;

            var user = _unitOfWork.Users.Get(id);
            return user;
        }

        // POST: api/<controller>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        // PUT: api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
