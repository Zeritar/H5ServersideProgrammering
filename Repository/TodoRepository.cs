using H5ServersideProgrammering.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace H5ServersideProgrammering.Repository
{
    public interface ITodoRepository
    {
        public List<TodoItem> GetAll();
        public TodoItem? GetById(int id);
        public List<TodoItem> GetByUserId(string userId);
        public int Add(TodoItem todoItem);
        public void Update(TodoItem todoItem);
        public bool Delete(int id);
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
            return _context.TodoItems.AsNoTracking().ToList();
        }

        public TodoItem? GetById(int id)
        {
            return _context.TodoItems.AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public List<TodoItem> GetByUserId(string userId)
        {
            return _context.TodoItems.AsNoTracking().Where(t => t.UserId == userId).ToList();
        }

        public int Add(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
            _context.Entry(todoItem).State = EntityState.Detached;
            return todoItem.Id;
        }

        public void Update(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var todoItem = _context.TodoItems.FirstOrDefault(x => x.Id == id);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
