using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Pages.Todos
{
    public class EditModel : PageModel
    {
        private readonly TodoContext _context;

        public EditModel(TodoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            Todo = todo;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var todoToUpdate = await _context.Todos.FindAsync(Todo.Id);
            if (todoToUpdate == null)
            {
                return NotFound();
            }

            // Update completion status and timestamp
            if (Todo.IsCompleted && !todoToUpdate.IsCompleted)
            {
                todoToUpdate.CompletedAt = DateTime.UtcNow;
            }
            else if (!Todo.IsCompleted && todoToUpdate.IsCompleted)
            {
                todoToUpdate.CompletedAt = null;
            }

            todoToUpdate.Title = Todo.Title;
            todoToUpdate.Description = Todo.Description;
            todoToUpdate.IsCompleted = Todo.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(Todo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}