using System.ComponentModel.DataAnnotations;
using FlightPlanner.Core.Interfaces;

namespace if_scooters.core.Models;

public abstract class Entity :IEntity
{
    [Key]
    public int Id { get; set; }
}
