namespace MatchingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService userService;

        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        [HttpGet("get-id/{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var res = await userService.GetUserAsync(id);
            return (res is null) ? NotFound() : Ok(res);
        }

        [HttpGet("get-email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var res = await userService.GetUserByEmailAsync(email);
            return (res is null) ? NotFound() : Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            var res = await userService.CreateUserAsync(user);
            return res.Success ? Ok() : BadRequest(res.ErrorMessage);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            var res = await userService.UpdateUserAsync(user);
            return res.Success ? Ok() : NotFound();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] User user)
        {
            var res = await userService.DeleteUserAsync(user);
            return (res.Success) ? Ok() : NotFound();
        }
    }
}
