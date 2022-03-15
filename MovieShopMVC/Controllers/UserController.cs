using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController: Controller
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;

        public UserController(ICurrentUser currentUser, IUserService userService, IMovieService movieService)
        {
            _currentUser = currentUser;
            _userService = userService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            var userId = _currentUser.UserId;
            var purchaseList = await _userService.GetAllPurchasesForUser(userId);
            return View(purchaseList);
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = _currentUser.UserId;
            var favoritesList = await _userService.GetAllFavoritesForUser(userId);
            return View(favoritesList);
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var userId = _currentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyMovie(int movieId)
        {
            var userId = _currentUser.UserId;
            var moviePrice = await _movieService.GetMoviePrice(movieId);
           // var moviePrice = 
            var purchaseRequest = new PurchaseRequestModel {
                MovieId = movieId,
                UserId = userId,
                PurchaseNumber = Guid.NewGuid(),
                TotalPrice = moviePrice,
                PurchaseDateTime = DateTime.UtcNow
            };
            var purchase = await _userService.PurchaseMovie(purchaseRequest, userId);
            return RedirectToAction("Purchases");
        }

        [HttpPost]
        public async Task<IActionResult> FavoriteMovie()
        {
            var userId = _currentUser.UserId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReviewMovie(ReviewRequestModel reviewRequest)
        {
            var userId = _currentUser.UserId;
            await _userService.AddMovieReview(reviewRequest);
            return View();
        }
    }
}
