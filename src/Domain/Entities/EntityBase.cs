using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Base domain class
    /// </summary>
    public abstract class EntityBase
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]

        public string Name { get; set; }

        public string Summary { get; set; }

        public Uri ImageUri { get; set; }
    }
}