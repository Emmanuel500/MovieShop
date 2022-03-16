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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<List<GenreModel>> GetGenres()
        {
            var allGenre = await _genreRepository.GetAll();
            var genreModel = new List<GenreModel>();
            // mapping entities data in to models data
            foreach (var genre in allGenre)
                genreModel.Add(new GenreModel
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            return genreModel;
        }
    }
}
