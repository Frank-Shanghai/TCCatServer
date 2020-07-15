using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace TCCatServer.Controllers
{
    [Authorize]
    [RoutePrefix("api/Roles")]
    public class RolesController : BaseApiController
    {
        // GET: api/Roles
        public IEnumerable<IdentityRole> Get()
        {
            return this.AppRoleManager.Roles;
        }

        // GET: api/Role/5
        public async Task<IHttpActionResult> Get(string id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return Ok(role);
            }

            return NotFound();
        }

        // POST: api/Role
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IHttpActionResult> Post(IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await this.AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                //return BadRequest(ModelState);
                return this.GetErrorResult(result);
            }

            return CreatedAtRoute("DefaultApi", new { id = role.Id }, role);
        }

        // PUT: api/Role/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IHttpActionResult> Put(string id, IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.Id)
            {
                return BadRequest();
            }

            var result = await this.AppRoleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Role/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var result = await this.AppRoleManager.DeleteAsync(this.AppRoleManager.FindById(id));
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return Ok();
        }
    }
}
