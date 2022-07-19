using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HigherPercentile.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Symbol { get; set; }
        public double Price { get; set; }
        public double OneMonthReturn { get; set; }
        public double ThreeMonthReturn { get; set; }
        public double SixMonthReturn { get; set; }
        public double OneYearReturn { get; set; }
        public double MomentumTotal { get; set; }

        public Stock(string symbol, string name)
        {
            Name = name;
            Symbol = symbol;
        }

        public override string? ToString()
        {
            return $"{Id}, {Name}, {Price}, {OneMonthReturn}, {ThreeMonthReturn}, {SixMonthReturn}, {OneYearReturn}, {MomentumTotal}"; ;
        }

        //public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        //[DisplayName("Display Order")]
    }
}