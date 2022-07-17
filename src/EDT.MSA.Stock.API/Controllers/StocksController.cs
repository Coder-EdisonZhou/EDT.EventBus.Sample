using AutoMapper;
using EDT.MSA.Stocking.API.Models;
using EDT.MSA.Stocking.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StocksController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IList<StockVO>>> GetAllStocks()
        {
            var stocks = await _stockService.GetAllStocks();
            return Ok(_mapper.Map<IList<StockVO>>(stocks));
        }

        [HttpGet("id")]
        public async Task<ActionResult<StockVO>> GetStock(string id)
        {
            var stock = await _stockService.GetStock(id);
            if (stock == null)
                return NotFound();

            return Ok(_mapper.Map<StockVO>(stock));
        }

        [HttpPost]
        public async Task<ActionResult<StockVO>> CreateStock(StockDTO stockDTO)
        {
            var stock = _mapper.Map<Stock>(stockDTO);
            stock.CreatedDate = DateTime.Now;
            stock.UpdatedDate = stock.CreatedDate;
            await _stockService.CreateStock(stock);

            return CreatedAtAction(nameof(GetStock), new { id = stock.ProductId }, _mapper.Map<StockVO>(stock));
        }
    }
}
