using MotorX.Api.DTOs.Requests.Queries;
using MotorX.Api.DTOs.Responses;
using MotorX.Api.Services;

namespace MotorX.Api.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<IEnumerable<T>> CreatePagedReponse<T>(IEnumerable<T> pagedData, PaginationQuery validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new PagedResponse<IEnumerable<T>>(
                pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationQuery(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationQuery(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationQuery(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationQuery(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
