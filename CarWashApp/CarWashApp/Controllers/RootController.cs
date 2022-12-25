using CarWashApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<Link>> Get()
        {
            List<Link> links = new List<Link>();

            links.Add(new Link(href: Url.Link("getRoot", new {}), rel: "self", method: "GET"));
            links.Add(new Link(href: Url.Link("registerUser", new{}), rel: "register-user", method: "POST"));
            links.Add(new Link(href: Url.Link("loginUser", new { }), rel: "login-user", method: "POST"));
            links.Add(new Link(href: Url.Link("getCarWashes", new PaginationDTO()), rel: "get-all-carwashes", method: "GET"));

            return links;
        }
    }
}
