using AutoMapper;
using DotNetCore.CAP;
using EDT.MSA.Stocking.API.Models;
using EDT.MSA.Stocking.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _eventPublisher;

        public StocksController(IStockRepository stockRepository, IMapper mapper, ICapPublisher eventPublisher)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IList<StockVO>>> GetAllStocks()
        {
            var stocks = await _stockRepository.GetAllStocks();
            return Ok(_mapper.Map<IList<StockVO>>(stocks));
        }

        [HttpGet("id")]
        public async Task<ActionResult<StockVO>> GetStock(string id)
        {
            var stock = await _stockRepository.GetStock(id);
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
            await _stockRepository.CreateStock(stock);

            return CreatedAtAction(nameof(GetStock), new { id = stock.ProductId }, _mapper.Map<StockVO>(stock));
        }
    }
}
