using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
    public class Company : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Bs { get; set; }
    }
}