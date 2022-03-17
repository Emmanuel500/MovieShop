using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IMovieRepository: IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetTop30RevenueMovies();
        Task<IEnumerable<Movie>> GetTop30RatingMovies();
        Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1);
        Task<PagedResultSet<Movie>> GetAllMovies(int pageSize = 30, int pageNumber = 1);
        Task<decimal> GetMoviePrice(int movieId);
    }
}
