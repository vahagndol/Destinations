namespace Domain.Entities
{
    /// <summary>
    /// Location domain class
    /// </summary>
    /// <seealso cref="Domain.Entities.EntityBase" />
    public class Location : EntityBase
    {
        public Location(int id, string name)
        {
            //TODO: perform validations in ctor
            Id = id;
            Name = name;
        }
    }
}
