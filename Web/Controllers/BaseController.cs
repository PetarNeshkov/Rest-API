using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class BaseApiController : ControllerBase;