using WebApplication.Data.Entities;

namespace WebApplication.Domain.Services
{
    public interface IMovieService
    {
        Movie Get(int id);
    }
}