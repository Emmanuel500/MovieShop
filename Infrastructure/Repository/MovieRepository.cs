using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovieRepository: EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> GetTop30RevenueMovies()
        {
            // get top 30 movies by revenue
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetTop30RatingMovies()
        {
            var ratedMovies = await _dbContext.Movies.Include(m => m.Reviews).OrderByDescending(m => m.Reviews.Sum(r => r.Rating)).Take(30).ToListAsync();
            return ratedMovies;
        }

        public override async Task<Movie> GetById(int id)
        {
            // First throw ex if no matches found
            // FirstOrDefault safest
            // Single throw ex 0 or more than 1
            // SingleOrDefault throw ex if more than 1
            // we need to use Include method
            var movieDetails = await _dbContext.Movies.Include(m => m.Genres).ThenInclude(m => m.Genre)
                .Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            var movieRating = await _dbContext.Reviews.Where(r => r.MovieId == id).DefaultIfEmpty()
            .AverageAsync(r => r == null ? 0 : r.Rating);
            movieDetails.Rating = movieRating;

            return movieDetails;
        }

        public async Task<PagedResultSet<Movie>> GetAllMovies(int pageSize = 30, int pageNumber = 1)
        {
            // get total movies count
            var totalMoviesCount = await _dbContext.Movies.CountAsync();

            if (totalMoviesCount == 0)
            {
                throw new Exception("No Movies Found for that genre");
            }


            var movies = await _dbContext.Movies
                .OrderBy(m => m.Id)
                .Select(m => new Movie
                {
                    Id = m.Id,
                    PosterUrl = m.PosterUrl,
                    Title = m.Title
                })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCount);
            return pagedMovies;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize=30, int pageNumber=1)
        {
            // get total movies count for that genre
            var totalMoviesCountByGenre = await _dbContext.MovieGenres.Where(m => m.GenreId == genreId).CountAsync();

            // get the actual movies from MovieGenre and Movie Table
            if (totalMoviesCountByGenre == 0)
            {
                throw new Exception("No Movies Found for that genre");
            }


            var movies = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(m => m.Movie)
                .OrderBy(m => m.MovieId)
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCountByGenre);
            return pagedMovies;
        }

        public async Task<PagedResultSet<Review>> GetAllReviewsOfMovie(int movieId, int pageSize = 30, int pageNumber = 1)
        {
            // get total movie reviews
            var totalMovieReviews = await _dbContext.Reviews.Where(m => m.MovieId == movieId).CountAsync();

            // get the actual movies from MovieGenre and Movie Table
            if (totalMovieReviews == 0)
            {
                throw new Exception("No Reviews Found for that movie");
            }


            var reviews = await _dbContext.Reviews.Where(g => g.MovieId == movieId)
                .Select(m => new Review
                {
                    UserId = m.UserId,
                    MovieId = m.MovieId,
                    Rating = m.Rating,
                    ReviewText = m.ReviewText
                })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Review>(reviews, pageNumber, pageSize, totalMovieReviews);
            return pagedMovies;
        }

        public async Task<decimal> GetMoviePrice(int movieId)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            return movie.Price.GetValueOrDefault();
            
        }
    }
}
