using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService: IMovieService
    {

        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieCardModel>> GetTop30GrossingMovies()
        {
            var movies = await _movieRepository.GetTop30RevenueMovies();
            var movieCards = new List<MovieCardModel>();

            // mapping entities data in to models data
            foreach (var movie in movies)
                movieCards.Add(new MovieCardModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title
                });

            return movieCards;
        }

        public async Task<List<MovieCardModel>> GetTop30RatingMovies()
        {
            var movies = await _movieRepository.GetTop30RatingMovies();
            var movieCards = new List<MovieCardModel>();

            // mapping entities data in to models data
            foreach (var movie in movies)
                movieCards.Add(new MovieCardModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title
                });

            return movieCards;
        }

        public async Task<MovieDetailsModel> GetMovieDetails(int id)
        {
            var movie = await _movieRepository.GetById(id);

            var movieDetails = new MovieDetailsModel
            {
                Id = movie.Id,
                Price = movie.Price,
                Budget = movie.Budget,
                Overview = movie.Overview,
                Revenue = movie.Revenue,
                Tagline = movie.Tagline,
                Title = movie.Title,
                ImdbUrl = movie.ImdbUrl,
                RunTime = movie.RunTime,
                BackdropUrl = movie.BackdropUrl,
                PosterUrl = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate,
                TmdbUrl = movie.TmdbUrl,
                Rating = movie.Rating
            };

            movieDetails.Genres = new List<GenreModel>();
            foreach (var genre in movie.Genres)
                movieDetails.Genres.Add(new GenreModel
                {
                    Id = genre.GenreId,
                    Name = genre.Genre.Name
                });

            movieDetails.Casts = new List<CastModel>();
            foreach (var cast in movie.MovieCasts)
                movieDetails.Casts.Add(new CastModel
                {
                    Id = cast.CastId,
                    Name = cast.Cast.Name,
                    Character = cast.Character,
                    ProfilePath = cast.Cast.ProfilePath
                });

            movieDetails.Trailers = new List<TrailerModel>();
            foreach (var trailer in movie.Trailers)
                movieDetails.Trailers.Add(new TrailerModel
                {
                    Id = trailer.Id,
                    Name = trailer.Name,
                    TrailerUrl = trailer.TrailerUrl
                });

            movieDetails.Reviews = new List<ReviewRequestModel>();
            foreach (var review in movie.Reviews)
            {
                movieDetails.Reviews.Add(new ReviewRequestModel
                {
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return movieDetails;
        }

        public async Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            var pagedMovies = await _movieRepository.GetMoviesByGenres(genreId, pageSize, pageNumber);

            var movieCards = new List<MovieCardModel>();

            movieCards.AddRange(pagedMovies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                PosterUrl = m.PosterUrl,
                Title = m.Title
            }));

            return new PagedResultSet<MovieCardModel>(movieCards, pageNumber, pageSize, pagedMovies.Count);
        }

        public async Task<PagedResultSet<MovieCardModel>> GetAllMoviesPagination(int pageSize = 30, int pageNumber = 1)
        {
            var pagedMovies = await _movieRepository.GetAllMovies(pageSize, pageNumber);

            var movieCards = new List<MovieCardModel>();

            movieCards.AddRange(pagedMovies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                PosterUrl = m.PosterUrl,
                Title = m.Title
            }));

            return new PagedResultSet<MovieCardModel>(movieCards, pageNumber, pageSize, pagedMovies.Count);
        }

        public async Task<PagedResultSet<ReviewRequestModel>> GetMovieReviews(int movieId, int pageSize = 30, int pageNumber = 1)
        {
            var pagedReviews = await _movieRepository.GetAllReviewsOfMovie(movieId, pageSize, pageNumber);

            var movieReviews = new List<ReviewRequestModel>();

            movieReviews.AddRange(pagedReviews.Data.Select(m => new ReviewRequestModel
            {
                UserId = m.UserId,
                MovieId = m.MovieId,
                Rating = m.Rating,
                ReviewText = m.ReviewText
            }));

            return new PagedResultSet<ReviewRequestModel>(movieReviews, pageNumber, pageSize, pagedReviews.Count);
        }

        public async Task<decimal> GetMoviePrice(int movieId)
        {
            return await _movieRepository.GetMoviePrice(movieId);
        }
    }
}
