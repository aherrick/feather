using feather;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/api/todos", TodoHttpService.GetTodos);
app.MapGet("/api/todos/{id}", TodoHttpService.GetTodo);
app.MapPost("/api/todos", TodoHttpService.CreateTodo);
app.MapPost("/api/todos/{id}", TodoHttpService.UpdateCompleted);
app.MapDelete("/api/todos/{id}", TodoHttpService.DeleteTodo);

await app.RunAsync();