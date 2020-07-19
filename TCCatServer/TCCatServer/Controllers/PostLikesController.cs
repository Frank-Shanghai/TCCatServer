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
    public class PostLikesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PostLikes
        public IQueryable<PostLike> GetPostLikes()
        {
            return db.PostLikes;
        }

        // GET: api/PostLikes/5
        [ResponseType(typeof(PostLike))]
        public async Task<IHttpActionResult> GetPostLike(Guid id)
        {
            PostLike postLike = await db.PostLikes.FindAsync(id);
            if (postLike == null)
            {
                return NotFound();
            }

            return Ok(postLike);
        }

        // PUT: api/PostLikes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPostLike(Guid id, PostLike postLike)
        {
            // users can only modify their own post likes
            if (postLike.User.UserName != User.Identity.Name)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != postLike.Id)
            {
                return BadRequest();
            }

            db.Entry(postLike).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostLikeExists(id))
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

        // POST: api/PostLikes
        [ResponseType(typeof(PostLike))]
        public async Task<IHttpActionResult> PostPostLike(PostLike postLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PostLikes.Add(postLike);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostLikeExists(postLike.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = postLike.Id }, postLike);
        }

        // DELETE: api/PostLikes/5
        [Authorize(Roles = "SuperAdmin")]
        [ResponseType(typeof(PostLike))]
        public async Task<IHttpActionResult> DeletePostLike(Guid id)
        {
            PostLike postLike = await db.PostLikes.FindAsync(id);
            if (postLike == null)
            {
                return NotFound();
            }

            db.PostLikes.Remove(postLike);
            await db.SaveChangesAsync();

            return Ok(postLike);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostLikeExists(Guid id)
        {
            return db.PostLikes.Count(e => e.Id == id) > 0;
        }
    }
}