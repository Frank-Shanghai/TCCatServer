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
    public class ThreadFavoritesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ThreadFavorites
        public IQueryable<ThreadFavorite> GetThreadFavorites()
        {
            return db.ThreadFavorites;
        }

        // GET: api/ThreadFavorites/5
        [ResponseType(typeof(ThreadFavorite))]
        public async Task<IHttpActionResult> GetThreadFavorite(Guid id)
        {
            ThreadFavorite threadFavorite = await db.ThreadFavorites.FindAsync(id);
            if (threadFavorite == null)
            {
                return NotFound();
            }

            return Ok(threadFavorite);
        }

        // PUT: api/ThreadFavorites/5
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutThreadFavorite(Guid id, ThreadFavorite threadFavorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != threadFavorite.Id)
            {
                return BadRequest();
            }

            db.Entry(threadFavorite).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadFavoriteExists(id))
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

        // POST: api/ThreadFavorites
        [ResponseType(typeof(ThreadFavorite))]
        public async Task<IHttpActionResult> PostThreadFavorite(ThreadFavorite threadFavorite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ThreadFavorites.Add(threadFavorite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThreadFavoriteExists(threadFavorite.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = threadFavorite.Id }, threadFavorite);
        }

        // DELETE: api/ThreadFavorites/5
        [ResponseType(typeof(ThreadFavorite))]
        public async Task<IHttpActionResult> DeleteThreadFavorite(Guid id)
        {
            ThreadFavorite threadFavorite = await db.ThreadFavorites.FindAsync(id);
            if (threadFavorite == null)
            {
                return NotFound();
            }

            // users can only delete their own thread favorite
            if (threadFavorite.User.UserName != User.Identity.Name)
                return Unauthorized();

            db.ThreadFavorites.Remove(threadFavorite);
            await db.SaveChangesAsync();

            return Ok(threadFavorite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThreadFavoriteExists(Guid id)
        {
            return db.ThreadFavorites.Count(e => e.Id == id) > 0;
        }
    }
}