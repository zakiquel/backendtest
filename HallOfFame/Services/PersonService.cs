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