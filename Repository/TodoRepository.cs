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
        private readonly List<TodoItem> _todoItems = new();

        public List<TodoItem> GetAll()
        {
            return _todoItems;
        }

        public TodoItem? GetById(int id)
        {
            return _todoItems.FirstOrDefault(t => t.Id == id);
        }

        public List<TodoItem> GetByUserId(string userId)
        {
            return _todoItems.Where(t => t.UserId == userId).ToList();
        }

        public void Add(TodoItem todoItem)
        {
            
            int newId = _todoItems.Any() ? _todoItems.Max(t => t.Id) + 1 : 1;
            todoItem.Id = newId;
            _todoItems.Add(todoItem);
        }

        public void Update(TodoItem todoItem)
        {
            var existingItem = GetById(todoItem.Id);
            if (existingItem != null)
            {
          
                //existingItem.Text = todoItem.Text;
                //existingItem.IsComplete = todoItem.IsComplete;
             
            }
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _todoItems.Remove(item);
            }
        }
    }
}
