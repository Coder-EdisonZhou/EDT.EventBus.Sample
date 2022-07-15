using System;
using System.ComponentModel.DataAnnotations;

namespace EDT.MSA.Stocking.API.Models
{
    public class StockDTO
    {
        [Required(ErrorMessage = "ProductId is necessary!")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "ProductName is necessary!")]
        public string ProductName { get; set; }
        [Range(1, 9999, ErrorMessage = "Valid range is 1 to 9999!")]
        public int StockQuantity { get; set; }
    }
}
