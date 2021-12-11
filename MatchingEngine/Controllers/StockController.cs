namespace MatchingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly StockService stockService;

        public StockController(ILogger<StockController> logger, StockService stockService)
        {
            _logger = logger;
            this.stockService = stockService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStocksAsync()
        {
            return Ok(await stockService.GetAllStocksAsync());
        }

        [HttpGet("get-id/{id}")]
        public async Task<IActionResult> GetStockAsynct(string id)
        {
            var res = await stockService.GetStockAsync(id);
            return (res is null) ? NotFound() : Ok(res);
        }

        [HttpGet("get-order-book/{id}")]
        public IActionResult GetStockOrderBook(string id)
        {
            var res = stockService.GetStockOrderBook(id);
            return (res is null) ? NotFound() : Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddStockAsync([FromBody] Stock stock)
        {
            var res = await stockService.AddStockAsync(stock);
            return (res.Success) ? Ok() : BadRequest(res.ErrorMessage);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStockAsync(string id)
        {
            var res = await stockService.DeleteStockAsync(id);
            return (res.Success) ? Ok() : NotFound();
        }
    }
}
