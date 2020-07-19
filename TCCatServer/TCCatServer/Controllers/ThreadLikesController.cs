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
    public class ThreadLikesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ThreadLikes
        public IQueryable<ThreadLike> GetThreadLikes()
        {
            return db.ThreadLikes;
        }

        // GET: api/ThreadLikes/5
        [ResponseType(typeof(ThreadLike))]
        public async Task<IHttpActionResult> GetThreadLike(Guid id)
        {
            ThreadLike threadLike = await db.ThreadLikes.FindAsync(id);
            if (threadLike == null)
            {
                return NotFound();
            }

            return Ok(threadLike);
        }

        // PUT: api/ThreadLikes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutThreadLike(Guid id, ThreadLike threadLike)
        {
            // Users can only modify their own thread like
            if (threadLike.User.UserName != User.Identity.Name)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != threadLike.Id)
            {
                return BadRequest();
            }

            db.Entry(threadLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadLikeExists(id))
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

        // POST: api/ThreadLikes
        [ResponseType(typeof(ThreadLike))]
        public async Task<IHttpActionResult> PostThreadLike(ThreadLike threadLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ThreadLikes.Add(threadLike);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThreadLikeExists(threadLike.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = threadLike.Id }, threadLike);
        }

        // DELETE: api/ThreadLikes/5
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(ThreadLike))]
        public async Task<IHttpActionResult> DeleteThreadLike(Guid id)
        {
            ThreadLike threadLike = await db.ThreadLikes.FindAsync(id);
            if (threadLike == null)
            {
                return NotFound();
            }

            db.ThreadLikes.Remove(threadLike);
            await db.SaveChangesAsync();

            return Ok(threadLike);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThreadLikeExists(Guid id)
        {
            return db.ThreadLikes.Count(e => e.Id == id) > 0;
        }
    }
}