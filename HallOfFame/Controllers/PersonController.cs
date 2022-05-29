using HallOfFame.Services;
using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    PersonService _service;
    
    public PersonController(PersonService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Person> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Person> GetById(int id)
    {
        var person = _service.GetById(id);

        if(person is not null)
        {
            return person;
        }
        else
        {
            return NotFound();
        }
    }


    [HttpPost]
    public IActionResult Create(Person newPerson)
    {
        var person = _service.Create(newPerson);
        return CreatedAtAction(nameof(GetById), new { id = person!.Id }, person);
    }

    [HttpPut("{id}/addskill")]
    public IActionResult AddSkill(int id, int skillId)
    {
        var personToUpdate = _service.GetById(id);

        if(personToUpdate is not null)
        {
            _service.AddSkill(id, skillId);
            return NoContent();    
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = _service.GetById(id);

        if(person is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}