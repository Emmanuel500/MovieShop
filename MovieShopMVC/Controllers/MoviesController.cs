using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController: Controller
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Details(int id)
        {
            // Movie Service with Details
            // pass the movie details data to view
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
