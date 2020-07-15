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
        // Web API 的返回类型知识
        // 返回的一个.net object类型，客户端怎么处理的？
        // web api controller自动处理了，在返回客户端之前会根据request 的accetp header的value使用相应media formatter做转化, 形成一个符合要求的http response message
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/action-results, 
        // 4种 void, HttpResponseMessage, IHttpActionRequest (可异步完成转换工作), Other type
        // asp.net core web api https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1

        // 像这个get all, 因为没有参数，所以不需要作validation，所以不用根据validation结果返回特定status code信息等，所以一般都是直接返回，但数据量大时有性能问题，可以参考上面最后
        // 一个链接，asp.net core web api已经做出了优化
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
        // 客户端传过来的数据，有的在url中，有的在data/body中，你这里的两个参数的情况，客户端应该怎么组织数据呢？
        // web api action parameter binding
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        //	• If the parameter is a "simple" type, Web API tries to get the value from the URI.Simple types include the.NET primitive types(int, bool, double, and so forth), 
        //    plus TimeSpan, DateTime, Guid,decimal, and string, plus any type with a type converter that can convert from a string. (More about type converters later.)
        //  • For complex types, Web API tries to read the value from the message body, using a media-type formatter.
        // https://www.tutorialsteacher.com/webapi/parameter-binding-in-web-api#:~:text=Parameter%20Binding%20%20%20%20HTTP%20Method%20,%20Complex%20Type%20%201%20more%20rows%20
        // 也可以用[FromUri], [FromBody]对参数binding作出限制，具体见上面链接

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
