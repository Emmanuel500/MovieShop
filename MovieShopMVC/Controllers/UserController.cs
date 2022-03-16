using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
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
            var reviewGet = await _userService.GetAllReviewsByUser(userId);
            return View(reviewGet);
        }

        [HttpPost]
        public async Task<IActionResult> BuyMovie(int movieId)
        {
            var userId = _currentUser.UserId;
            var moviePrice = await _movieService.GetMoviePrice(movieId);
            var purchaseRequest = new PurchaseRequestModel
            {
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
        public async Task<IActionResult> FavoriteMovie(int movieId)
        {
            var userId = _currentUser.UserId;
            var favoriteRequest = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };
            if (await _userService.FavoriteExists(userId, movieId))
            {
                await _userService.RemoveFavorite(favoriteRequest);
            }
            else
            {
                var favorite = await _userService.AddFavorite(favoriteRequest);
            }
            return RedirectToAction("Favorites");
        }

        [HttpPost]
        public async Task<IActionResult> ReviewMovie(int movieId, decimal rating, string reviewText)
        {
            var userId = _currentUser.UserId;
            var reviewRequest = new ReviewRequestModel
            {
                UserId = userId,
                MovieId = movieId,
                Rating = rating,
                ReviewText = reviewText
            };
            var revExist = await _userService.GetReviewDetails(userId, movieId);
            if (revExist == null)
            {
                await _userService.AddMovieReview(reviewRequest);
            }
            else
            {
                await _userService.UpdateMovieReview(reviewRequest);
            }

            return RedirectToAction("Reviews");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReviewMovie(int movieId)
        {
            var userId = _currentUser.UserId;
            var revExist = await _userService.GetReviewDetails(userId, movieId);
            if (revExist != null)
            {
                await _userService.DeleteMovieReview(userId, movieId);
            }

            return RedirectToAction("Reviews");
        }
    }
}
