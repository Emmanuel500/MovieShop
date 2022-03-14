using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Add
        public async Task<int> AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            var favorite = new Favorite
            {
                Id = favoriteRequest.Id,
                UserId = favoriteRequest.UserId,
                MovieId = favoriteRequest.MovieId
            };

            var createdFavorite = await _userRepository.AddFavorite(favorite);
            return createdFavorite.Id;
        }

        public async Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var review = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };

            var createdReview = await _userRepository.AddReview(review);

        }

        public async Task<int> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchase = new Purchase
            {
                Id = purchaseRequest.Id,
                UserId = userId,
                MovieId = purchaseRequest.MovieId,
                PurchaseNumber = purchaseRequest.PurchaseNumber,
                TotalPrice = purchaseRequest.TotalPrice,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime
            };

            var createdPurchase = await _userRepository.AddPurchase(purchase);
            return createdPurchase.Id;
        }

        //Delete
        public async Task DeleteMovieReview(int userId, int movieId)
        {
            await _userRepository.RemoveReview(userId, movieId);
        }

        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            await _userRepository.RemoveFavorite(favoriteRequest.Id, favoriteRequest.UserId, favoriteRequest.MovieId);
        }

        //Exist
        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            var favoriteExist = await _userRepository.UserFavoriteExist(id, movieId);
            return favoriteExist;
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var favoriteExist = await _userRepository.UserPurchaseExist(purchaseRequest.Id, userId);
            return favoriteExist;
        }

        //GetAll
        public async Task<List<FavoriteRequestModel>> GetAllFavoritesForUser(int id)
        {
            var favorites = await _userRepository.GetAllFavoritesFromUser(id);
            var favoriteList = new List<FavoriteRequestModel>();

            // mapping entities data in to models data
            foreach (var favorite in favorites)
                favoriteList.Add(new FavoriteRequestModel
                {
                    Id = favorite.Id,
                    UserId = favorite.UserId,
                    MovieId = favorite.MovieId
                });

            return favoriteList;
        }

        public async Task<List<PurchaseRequestModel>> GetAllPurchasesForUser(int id)
        {
            var purchases = await _userRepository.GetAllPurchasesFromUser(id);
            var purchaseList = new List<PurchaseRequestModel>();

            // mapping entities data in to models data
            foreach (var purchase in purchases)
                purchaseList.Add(new PurchaseRequestModel
                {
                    Id = purchase.Id,
                    UserId = purchase.UserId,
                    MovieId = purchase.MovieId,
                    PurchaseNumber = purchase.PurchaseNumber,
                    TotalPrice = purchase.TotalPrice,
                    PurchaseDateTime = purchase.PurchaseDateTime
                });

            return purchaseList;
        }

        public async Task<List<ReviewRequestModel>> GetAllReviewsByUser(int id)
        {
            var reviews = await _userRepository.GetAllReviewsFromUser(id);
            var reviewList = new List<ReviewRequestModel>();

            // mapping entities data in to models data
            foreach (var review in reviews)
                reviewList.Add(new ReviewRequestModel
                {
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });

            return reviewList;
        }

        //Get Detail
        public async Task<PurchaseRequestModel> GetPurchasesDetails(int userId, int movieId)
        {
            var purchase = await _userRepository.GetUserPurchase(userId, movieId);

            var purchaseDetails = new PurchaseRequestModel
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                MovieId = purchase.MovieId,
                PurchaseNumber = purchase.PurchaseNumber,
                PurchaseDateTime = purchase.PurchaseDateTime,
                TotalPrice = purchase.TotalPrice
            };

            return purchaseDetails;
        }

        //Update
        public async Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var review = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };

            var createdReview = await _userRepository.UpdateReview(review);
        }
    }
}
