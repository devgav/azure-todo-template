using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Data;

namespace TodoApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase {
        private readonly TodoContext _context;

        public TodoController(TodoContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id) {
            var todoItem = await _context.TodoItems.SingleOrDefaultAsync(todoItem => todoItem.Id == id);
            if (todoItem == null) {
                return NotFound();
            }
            return todoItem;
        }
        [HttpPost]
        public async Task<ActionResult<TodoItem>> AddTodoItem(TodoItem todoItem) {
            var todoItemList = await _context.TodoItems.ToListAsync();
            todoItem.Id = todoItemList.Count;
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> UpdateTodoItem(int id, TodoItem todoItem) {
            if (id != todoItem.Id) {
                return NotFound();
            }
            _context.Entry(todoItem).State = EntityState.Modified;
            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id) {
            var todoItem = await _context.TodoItems.SingleOrDefaultAsync(todoItem => todoItem.Id == id);
            if (todoItem == null) {
                return NotFound();
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}