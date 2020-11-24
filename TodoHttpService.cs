using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace feather
{
    public static class TodoHttpService
    {
        public static async Task GetTodos(HttpContext http)
        {
            using var db = new TodoDbContext();
            var todos = await db.Todos.ToListAsync();

            await http.Response.WriteAsJsonAsync(todos);
        }

        public static async Task GetTodo(HttpContext http)
        {
            if (!http.Request.RouteValues.TryGet("id", out int id))
            {
                http.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            using var db = new TodoDbContext();
            var todo = await db.Todos.FindAsync(id);
            if (todo == null)
            {
                http.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            await http.Response.WriteAsJsonAsync(todo);
        }

        public static async Task CreateTodo(HttpContext http)
        {
            var todo = await http.Request.ReadFromJsonAsync<TodoItem>();

            using var db = new TodoDbContext();
            await db.Todos.AddAsync(todo);
            await db.SaveChangesAsync();

            http.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        public static async Task UpdateCompleted(HttpContext http)
        {
            if (!http.Request.RouteValues.TryGet("id", out int id))
            {
                http.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            using var db = new TodoDbContext();
            var todo = await db.Todos.FindAsync(id);

            if (todo == null)
            {
                http.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var inputTodo = await http.Request.ReadFromJsonAsync<TodoItem>();
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync();

            http.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        public static async Task DeleteTodo(HttpContext http)
        {
            if (!http.Request.RouteValues.TryGet("id", out int id))
            {
                http.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            using var db = new TodoDbContext();
            var todo = await db.Todos.FindAsync(id);
            if (todo == null)
            {
                http.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            db.Todos.Remove(todo);
            await db.SaveChangesAsync();

            http.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }
    }
}