using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Domain
{
    public static class MessageStrings
    {
        public const string NoSearchCriteria = "No search criteria provided.";
        public const string NoResultsFound = "No results found";
        public const string NotFound = "Not found";
        public const string NoMoviesRatedByUser = "The user with Id:{0} didn't rate any movie";
        public const string MovieDbEmpty = "It looks like our movie database is empty";
        public const string MovieDoesNotExist = "Movie with id {0} doesn't exist.";
        public const string UserDoesNotExist = "User with id {0} doesn't exist.";
    }
}
