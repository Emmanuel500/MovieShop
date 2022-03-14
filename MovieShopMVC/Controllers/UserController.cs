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

        public UserController(ICurrentUser currentUser, IUserService userService)
        {
            _currentUser = currentUser;
            _userService = userService;
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
        public async Task<IActionResult> BuyMovie(PurchaseRequestModel purchaseRequest)
        {
            var userId = _currentUser.UserId;
            var purchase = await _userService.PurchaseMovie(purchaseRequest, userId);
            return View();
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
