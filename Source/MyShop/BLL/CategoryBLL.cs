using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BLL
{
    public class CategoryBLL
    {
        private CategoryDAL _categoryDAL;

        public CategoryBLL()
        {
            _categoryDAL = new CategoryDAL();
        }

        public CategoryDTO getCategoryById(int id)
        {
            return _categoryDAL.getById(id);
        }

        public ObservableCollection<CategoryDTO> getAllCategory()
        {
            return _categoryDAL.getAll();
        }

        public int addCategory(CategoryDTO category)
        {
            return _categoryDAL.create(category);
        }

        public int updateCategory(CategoryDTO category)
        {
            return _categoryDAL.update(category);
        }

        public int deleteCategory(int id)
        {
            return _categoryDAL.delete(id);
        }

    }
}
