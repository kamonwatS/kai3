using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Pages.Todos
{
    public class CreateModel : PageModel
    {
        private readonly TodoContext _context;

        public CreateModel(TodoContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Todo.CreatedAt = DateTime.UtcNow;
            _context.Todos.Add(Todo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}