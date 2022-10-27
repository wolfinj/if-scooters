using System.ComponentModel.DataAnnotations;
using if_scooters.core.Interfaces;
using if_scooters.core.Services;

namespace if_scooters.core.Models;

public abstract class Entity : IEntity
{
    [Key]
    public int Id { get; set; }
}
