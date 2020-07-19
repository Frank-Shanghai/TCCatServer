using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TCCatServer.Models;

namespace TCCatServer.Controllers
{
    [Authorize]
    public class ForaController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Fora
        public IQueryable<Forum> GetForums()
        {
            return db.Forums;
        }

        // GET: api/Fora/5
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> GetForum(Guid id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            return Ok(forum);
        }

        // PUT: api/Fora/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForum(Guid id, Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forum.Id)
            {
                return BadRequest();
            }

            db.Entry(forum).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Fora
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> PostForum(Forum forum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Forums.Add(forum);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ForumExists(forum.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = forum.Id }, forum);
        }

        // DELETE: api/Fora/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Forum))]
        public async Task<IHttpActionResult> DeleteForum(Guid id)
        {
            Forum forum = await db.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }

            db.Forums.Remove(forum);
            await db.SaveChangesAsync();

            return Ok(forum);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ForumExists(Guid id)
        {
            return db.Forums.Count(e => e.Id == id) > 0;
        }
    }
}