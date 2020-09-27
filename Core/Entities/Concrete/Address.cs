using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public class Address : IEntity
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public int? GeolocationID { get; set; }

        public virtual Geolocation Geo { get; set; }
    }
}