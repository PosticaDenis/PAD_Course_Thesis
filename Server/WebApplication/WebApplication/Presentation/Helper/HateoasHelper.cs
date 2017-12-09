using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiskFirst.Hateoas;
using RiskFirst.Hateoas.Models;

namespace WebApplication.Presentation.Helper
{
    public static class HateoasHelper
    {
        public static async Task<IEnumerable<T>> AddLinks<T>(this IEnumerable<T> enumerable, ILinksService linksService)
            where T : ILinkContainer
        {
            enumerable = enumerable.ToList();
            foreach (var it in enumerable)
            {
                await linksService.AddLinksAsync(it);
            }

            return enumerable;
        }
        
        public static async Task<T> AddLinks<T>(this T linkContainer, ILinksService linksService)
            where T : ILinkContainer
        {
            await linksService.AddLinksAsync(linkContainer);

            return linkContainer;
        }
    }
}