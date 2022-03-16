using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    public class MoviesController: Controller
    {
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        public MoviesController(IMovieService movieService, IUserService userService, ICurrentUser currentUser)
        {
            _movieService = movieService;
            _userService = userService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Movie Service with Details
            // pass the movie details data to view
            // check if user id loged
            // is MoviePurcahsed (userid, movieid) => trur /false
            if (_currentUser.IsAuthenticated)
            {
                var moviePurchased = await _userService.IsMoviePurchased(id, _currentUser.UserId);
                var movieFavorited = await _userService.FavoriteExists(_currentUser.UserId, id);
                var movieRewview = await _userService.GetReviewDetails(_currentUser.UserId, id);
                ViewBag.MoviePurchased = moviePurchased;
                ViewBag.MovieFavorited = movieFavorited;
                if (movieRewview != null)
                {
                    ViewBag.Rating = movieRewview.Rating;
                    ViewBag.ReviewText = movieRewview.ReviewText;
                }
                else
                {
                    ViewBag.Rating = 0;
                    ViewBag.ReviewText = "";
                }
            }
            else
            {
                ViewBag.MoviePurchased = false;
                ViewBag.MovieFavorited = false;
                ViewBag.Rating = 0;
                ViewBag.ReviewText = "";
            }
                       
            var movieDetails = await _movieService.GetMovieDetails(id);
            return View(movieDetails);
        }

        [HttpGet]
        public async Task<IActionResult> MoviesByGenre(int id, int pageSize=30, int pageNumber=1)
        {
            var pagedMovies = await _movieService.GetMoviesByGenrePagination(id, pageSize, pageNumber);
            return View("PagedMovies", pagedMovies);
        }
    }
}
