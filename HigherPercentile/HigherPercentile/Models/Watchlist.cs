using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HigherPercentile.Models
{
    public class Watchlist
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("User")]
        public int? UserId{ get; set; }

        [ForeignKey("Stock")]
        public int? StockId{ get; set; }
    }
}
