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
    public class ThreadsController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Threads
        public IQueryable<Thread> GetThreads()
        {
            return db.Threads;
        }

        // GET: api/Threads/5
        [ResponseType(typeof(Thread))]
        public async Task<IHttpActionResult> GetThread(Guid id)
        {
            Thread thread = await db.Threads.FindAsync(id);
            if (thread == null)
            {
                return NotFound();
            }

            return Ok(thread);
        }

        // PUT: api/Threads/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutThread(Guid id, Thread thread)
        {
            // Users can only modify their own threads
            if (thread.Author.UserName != User.Identity.Name)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != thread.Id)
            {
                return BadRequest();
            }

            db.Entry(thread).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(id))
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

        // POST: api/Threads
        [ResponseType(typeof(Thread))]
        public async Task<IHttpActionResult> PostThread(Thread thread)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Threads.Add(thread);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThreadExists(thread.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = thread.Id }, thread);
        }

        // DELETE: api/Threads/5
        [ResponseType(typeof(Thread))]
        public async Task<IHttpActionResult> DeleteThread(Guid id)
        {
            Thread thread = await db.Threads.FindAsync(id);

            if (thread == null)
            {
                return NotFound();
            }

            // Users can only modify their own threads
            if (thread.Author.UserName != User.Identity.Name)
                return Unauthorized();

            db.Threads.Remove(thread);
            await db.SaveChangesAsync();

            return Ok(thread);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThreadExists(Guid id)
        {
            return db.Threads.Count(e => e.Id == id) > 0;
        }
    }
}