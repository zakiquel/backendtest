using System.ComponentModel.DataAnnotations;

namespace HallOfFame.Models;

public class Person
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? DisplayName { get; set; }
    
    public ICollection<Skill>? Skills { get; set; }
}