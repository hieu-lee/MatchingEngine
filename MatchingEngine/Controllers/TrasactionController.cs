namespace MatchingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrasactionController : ControllerBase
    {
        private readonly ILogger<TrasactionController> _logger;
        private readonly TransactionService transactionService;

        public TrasactionController(ILogger<TrasactionController> logger, TransactionService transactionService)
        {
            _logger = logger;
            this.transactionService = transactionService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTransactionsAsync()
        {
            return Ok(await transactionService.GetAllTransactionsAsync());
        }

        [HttpGet("get-buy/{userId}")]
        public async Task<IActionResult> GetUserBuyTransactionsAsync(string userId)
        {
            return Ok(await transactionService.GetUserBuyTransactionsAsync(userId));
        }

        [HttpGet("get-sell/{userId}")]
        public async Task<IActionResult> GetUserSellTransactionsAsync(string userId)
        {
            return Ok(await transactionService.GetUserSellTransactionsAsync(userId));
        }
    }
}
