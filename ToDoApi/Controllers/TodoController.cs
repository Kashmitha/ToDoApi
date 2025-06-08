using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/todo
        [HttpGet]
        public ActionResult<IEnumerable<TodoItemDto>> GetAll()
        {
            var items =  _context.TodoItems.ToList();
            return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(items));
        }

        // GET: api/todo/{id}
        [HttpGet("{id}")]
        public ActionResult<TodoItemDto> GetById(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<TodoItemDto>(item));
        }

        // POST: api/todo
        [HttpPost]
        public ActionResult<TodoItemDto> Create(CreateTodoDto createDto)
        {
            var todoItem = _mapper.Map<TodoItem>(createDto);
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            var readDto = _mapper.Map<TodoItemDto>(todoItem);
            return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
        }

        // PUT: api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, CreateTodoDto updateDto)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            _mapper.Map(updateDto, item);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
