using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Models.Entities;
using TaskManager.Models.DAL;

namespace TaskManager.Models.Services
{
    public class CategoryService : Service<Category>
    {
        public CategoryService(IRepository<Category> repository) : base(repository) { }

        public void AddCategory(Category category)
        {
            category.Id = Guid.NewGuid();
            _repository.Add(category);
            _repository.Save();
        }

        public void UpdateCategory(Guid id, Category category)
        {
            Category native = _repository.GetById(id);
            native = category;
            _repository.Update(native);
            _repository.Save();
        }

        public void DeleteCategory (Guid id)
        {
            _repository.Delete(_repository.GetById(id));
            _repository.Save();
        }

    }
}