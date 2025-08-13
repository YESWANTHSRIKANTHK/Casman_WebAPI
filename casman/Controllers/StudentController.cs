using casman.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentDbContext _context;

    public StudentsController(StudentDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Get() =>
        await _context.Students.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> Get(int id)
    {
        var Student = await _context.Students.FindAsync(id);
        if (Student == null) return NotFound();
        return Student;
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Post(Student Student)
    {
        _context.Students.Add(Student);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = Student.ID }, Student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Student Student)
    {
        if (id != Student.ID) return BadRequest();
        _context.Entry(Student).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var Student = await _context.Students.FindAsync(id);
        if (Student == null) return NotFound();
        _context.Students.Remove(Student);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
