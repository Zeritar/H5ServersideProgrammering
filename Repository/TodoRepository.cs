using H5ServersideProgrammering.Data;
using System.Collections.Generic;
using System.Linq;

namespace H5ServersideProgrammering.Repository
{
    public interface ITodoRepository
    {
        public List<TodoItem> GetAll();
        public TodoItem? GetById(int id);
        public List<TodoItem> GetByUserId(string userId);
        public void Add(TodoItem todoItem);
        public void Update(TodoItem todoItem);
        public void Delete(int id);
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly AppDataContext _context;

        public TodoRepository(AppDataContext context)
        {
            _context = context;
        }

        public List<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        public TodoItem? GetById(int id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public List<TodoItem> GetByUserId(string userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId).ToList();
        }

        public void Add(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public void Update(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var todoItem = _context.TodoItems.First(x => x.Id == id);
            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
        }
    }
}
