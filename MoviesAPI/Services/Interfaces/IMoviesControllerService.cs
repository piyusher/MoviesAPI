﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Domain;
using MoviesAPI.Domain.RequestModels;

namespace MoviesAPI.Services.Interfaces
{
    public interface IMoviesControllerService
    {
        Task<List<MovieModelWithAvgRating>> SearchMoviesAsync(string title, int year, string genres);
        Task<List<MovieModelWithAvgRating>> GetTopRatedMovies();
        Task<List<MovieModelWithAvgRating>> GetTopMoviesRatedByUser(long userId);
    }
}
