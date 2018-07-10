namespace Domain.Entities
{
    /// <summary>
    /// Place domain class
    /// </summary>
    /// <seealso cref="Domain.Entities.EntityBase" />
    public class Place : EntityBase
    {
        public int CityId { get; set; }

        public Place(int id, int cityId, string name)
        {
            //TODO: perform validations in ctor
            Id = id;
            CityId = cityId;
            Name = name;            
        }
    }
}
