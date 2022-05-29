using HallOfFame.Models;
using HallOfFame.Data;
using Microsoft.EntityFrameworkCore;


namespace HallOfFame.Services;

public class PersonService
{
    private readonly PersonContext _context;


   public PersonService(PersonContext context)
    {
        _context = context;
    }

    public IEnumerable<Person> GetAll()
    {
        return _context.Persons
            .Include(p => p.Skills)
            .AsNoTracking()
            .ToList();
    }

    public Person? GetById(int id)
    {
        return _context.Persons
            .Include(p => p.Skills)
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Person Create(Person newPerson)
    {
        _context.Persons.Add(newPerson);
        _context.SaveChanges();

        return newPerson;
    }

    public void AddSkill(int PersonId, int SkillId)
    {
        var personToUpdate = _context.Persons.Find(PersonId);
        var skillToAdd = _context.Skills.Find(SkillId);

        if (personToUpdate is null || skillToAdd is null)
        {
            throw new NullReferenceException("Person or skill does not exist");
        }

        if(personToUpdate.Skills is null)
        {
            personToUpdate.Skills = new List<Skill>();
        }

            personToUpdate.Skills.Add(skillToAdd);

            _context.Persons.Update(personToUpdate);
            _context.SaveChanges();
    }


   public void DeleteById(int id)
    {
        var personToDelete = _context.Persons.Find(id);
        if (personToDelete is not null)
        {
            _context.Persons.Remove(personToDelete);
            _context.SaveChanges();
        }        
    }
}