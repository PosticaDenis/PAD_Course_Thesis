using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Mapper
{
    public static class ActorMapper
    {
        public static Actor Map(Data.Entities.Actor source, Actor destination)
        {
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.Id = source.Id;

            return destination;
        }

        public static Actor Map(Data.Entities.Actor source)
        {
            return Map(source, new Actor());
        }
        
        public static Data.Entities.Actor Map(Actor source, Data.Entities.Actor destination)
        {
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            
            return destination;
        }
        
        public static Data.Entities.Actor Map(Actor source)
        {
            return Map(source, new Data.Entities.Actor());
        }
    }
}