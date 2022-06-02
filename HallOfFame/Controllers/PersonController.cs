using HallOfFame.Services;
using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HallOfFame.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


using Microsoft.Extensions.Logging;


namespace HallOfFame.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PersonController : ControllerBase
{
    PersonService _service;
    private readonly PersonContext _context;
    public PersonController(PersonService service, PersonContext context)
    {
        _service = service;
        _context = context;
    }

    // GET: api/v1/persons Получение всех сотрудников
    [Route("~/api/v1/persons")]
    [HttpGet]
    public IEnumerable<Person> GetAll()
    {
        return _service.GetAll();
    }

    // GET: api/v1/person/id Получить конкретного сотрудника
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


    // POST: api/v1/person Добавление нового сотрудника
    [HttpPost]
    public IActionResult Create(Person newPerson)
    {
        var person = _service.Create(newPerson);
        return CreatedAtAction(nameof(GetById), new { id = person!.Id }, person);
    }
    

    // PUT: api/v1/person/id Обновление данных конкретного сотрудника
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person updatedPerson)
        {
            if (id != updatedPerson.Id)
            {
                Dictionary<string, string> errors = new Dictionary<string, string>();
                errors["Others"] = "Неверный запрос";
                return BadRequest(errors);
            }


            var person = await _context.Persons.FindAsync(id);

            //обновляем поля у сотрудника
            _context.Entry(person).CurrentValues.SetValues(updatedPerson);

            //получаем текущие навыки сотрудника
            var personSkills = person.Skills.ToList();

            foreach (var personSkill in personSkills)
            {
                var skill = updatedPerson.Skills.SingleOrDefault(s => s.Name == personSkill.Name);
                if (skill != null)
                {
                    //обновляем поле у навыка сотрудника
                    _context.Entry(personSkill).CurrentValues.SetValues(skill);
                }
                else
                {
                    //удаляем, если навыка нет
                    _context.Remove(personSkill);
                }
            }

            //добавляем новые навыки
            foreach (var skill in updatedPerson.Skills)
            {
                if (personSkills.All(s => s.Name != skill.Name))
                {
                    person.Skills.Add(skill);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!PersonExists(id))
                {
                    Dictionary<string, string> errors = new Dictionary<string, string>();
                    errors["Others"] = "Сущность не найдена";
                    return NotFound(errors);
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return Ok(person);
        }




    // DELETE: api/v1/person/id  Удаление существующего сотрудника
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

    private bool PersonExists(long id)
        {
            return _context.Persons.Any(p => p.Id == id);
        }
}