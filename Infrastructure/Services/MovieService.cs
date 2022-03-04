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

        public List<MovieCardModel> GetTop30GrossingMovies()
        {
            var movies = _movieRepository.GetTop30RevenueMovies();
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

        public MovieDetailsModel GetMovieDetails(int id)
        {
            var movie = _movieRepository.GetById(id);

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
                TmdbUrl = movie.TmdbUrl
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
                    Character = cast.Character,
                    Name = cast.Cast.Name,
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

            return movieDetails;
        }

    }
}
