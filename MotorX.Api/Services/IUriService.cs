using MotorX.Api.DTOs.Requests.Queries;

namespace MotorX.Api.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationQuery query, string route);
        public string GetBaseRoot();
    }
}
