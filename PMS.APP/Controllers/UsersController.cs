using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.APP.Context;
using PMS.APP.Models;

namespace PMS.APP.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly PMSContext _context;

        public UsersController(PMSContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(new {
                    status = true,
                    data = _context.Users
                });
                //return _context.Users;
            }
            catch
            {
                return BadRequest(new {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                var users = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);

                if (users == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }

                return Ok(new
                {
                    status = true,
                    data = users
                });
            }
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        // 
        [HttpPost("Login")]
        public async Task<IActionResult> GetUsersByEmail([FromBody()] Login user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                var users = new Users();
                if (user.UserType != null)
                    users = await _context.Users.FirstOrDefaultAsync(m => m.Email == user.Email && m.Password == user.Password && m.UserType.ToLower() == user.UserType.ToLower());
                else
                    users = await _context.Users.FirstOrDefaultAsync(m => m.Email == user.Email && m.Password == user.Password);

                if (users == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }
                return Ok(new
                {
                    status = true,
                    data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] Users users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                if (id != users.UserId)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                _context.Entry(users).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(id))
                    {
                        return NotFound(new
                        {
                            status = false,
                            message = HttpStatusCode.NotFound.ToString()
                        });
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new
                {
                    status = true,
                    data = users
                });
            }
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Users users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                if (UserInfoExists(users))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = users.Email + " already taken with same usertype."
                    });
                }

                _context.Users.Add(users);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUsers", new { id = users.UserId }, users);
            }
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = HttpStatusCode.BadRequest.ToString()
                    });
                }

                var users = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
                if (users == null)
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }

                _context.Users.Remove(users);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = true,
                    data = users
                });
            }
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool UserInfoExists(Users user)
        {
            return _context.Users.Any(e => e.Email == user.Email && e.UserType.ToLower() == user.UserType.ToLower());
        }
    }
}