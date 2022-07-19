using HigherPercentile.Data;
using HigherPercentile.Models;
using Microsoft.AspNetCore.Mvc;

namespace HigherPercentile.Controllers
{
    public class WatchlistController : Controller
    {
        ApplicationDbContext db;

        public WatchlistController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int userId)
        {
            if (userId==0) 
            {
                return RedirectToAction("Index", "Login");
            }

            List<Watchlist> fullWatchList = db.Watchlists.Where(x => x.UserId == userId).ToList();

            List<Stock> watchlistStocks = new List<Stock>();

            foreach (Watchlist watchlist in fullWatchList)
            {
                Stock? stock = db.Stocks.Find(watchlist.StockId);
                watchlistStocks.Add(stock);
            }

            ViewData["watchlist"] = watchlistStocks;

            User? user = db.Users.Find(userId);

            ViewData["user"] = user;

            return View();
        }

        //POST
        //must add these [] for the POST and to validate the integrety of the input
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Watchlist watchlistItemForm)
        {
            Console.WriteLine("***********here"+watchlistItemForm.StockId + " " + watchlistItemForm.UserId);

            IQueryable<Watchlist>? watchlistItem = db.Watchlists.Where(x => x.StockId == watchlistItemForm.StockId);

            db.Watchlists.RemoveRange(watchlistItem);
            db.SaveChanges();
            
            TempData["success"] = "Stock successfully deleted from Watchlist";

            return RedirectToAction("Index", "Watchlist", new { watchlistItemForm.UserId });
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Watchlist watchlistItem)
        {
           
            if (watchlistItem.UserId==null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            db.Watchlists.Add(watchlistItem);
            db.SaveChanges();
            TempData["success"] = "Category created successfully";
            
            return View();
        }   
        */
    }
}
