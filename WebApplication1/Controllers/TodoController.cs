using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class TodoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ApplicationDbContext context, ILogger<TodoController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Todo
    public async Task<IActionResult> Index()
    {
        var todoItems = await _context.TodoItems
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(todoItems);
    }

    // GET: Todo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Todo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] TodoItem todoItem)
    {
        if (ModelState.IsValid)
        {
            todoItem.CreatedAt = DateTime.UtcNow;
            _context.Add(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todoItem);
    }

    // POST: Todo/ToggleComplete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.IsCompleted = !todoItem.IsCompleted;
        todoItem.CompletedAt = todoItem.IsCompleted ? DateTime.UtcNow : null;
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: Todo/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
} 