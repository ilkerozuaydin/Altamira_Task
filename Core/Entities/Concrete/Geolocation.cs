using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public class Geolocation : IEntity
    {
        public int ID { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}