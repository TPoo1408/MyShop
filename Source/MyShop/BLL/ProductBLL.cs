using MyShop.DAL;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.BLL
{
    public class ProductBLL
    {
        private ProductDAL _productDAL;

        public ProductBLL()
        {
            _productDAL = new ProductDAL();
        }

        public ProductDTO getProductById(int id)
        {
            return _productDAL.getById(id);
        }

        public ObservableCollection<ProductDTO> getAllProduct(string? sortBy = null)
        {
            var result = _productDAL.getAll();
            return result;
        }

        public async Task<int> getTotalProduct()
        {
            return await _productDAL.countAll();
        }

        public async Task<ObservableCollection<ProductDTO>> getTop5ProductAlmostOutOfStock()
        {
            return await _productDAL.getTop5();
        }

        public async Task<Tuple<List<ProductDTO>, int>> findProductBySearch(int currentPage = 1, int productsPerPage = 9,
                string keyword = "", Decimal? startPrice = null, Decimal? endPrice = null, string? orderBy = null, bool asc = true, int type = -1)
        {
            var listProduct = _productDAL.getAll();
            var sortedList = listProduct.ToList<ProductDTO>();

            if (orderBy != null)
            {
                if (orderBy.ToLower() == "price")
                {
                    if (asc)
                    {
                        sortedList = listProduct.OrderBy(product => product.promotionPrice).ToList<ProductDTO>();
                    }
                    else
                    {
                        sortedList = listProduct.OrderByDescending(product => product.promotionPrice).ToList<ProductDTO>();
                    }
                }
                else if (orderBy.ToLower() == "name")
                {
                    if (asc)
                    {
                        sortedList = listProduct.OrderBy(product => product.name).ToList<ProductDTO>();
                    }
                    else
                    {
                        sortedList = listProduct.OrderByDescending(product => product.name).ToList<ProductDTO>();
                    }
                }
            }

            if (type != -1)
            {
                sortedList = sortedList.Where(product => product.categoryID == type).ToList<ProductDTO>();
            }

           
            var list = sortedList.Where((item) =>
            {
                bool checkName = item.name!.ToLower().Contains(keyword.ToLower());

                if (startPrice == null || endPrice == null) return checkName;

                bool checkPrice = item.price >= startPrice && item.price <= endPrice;

                return checkName && checkPrice;
            });

            var items = list.Skip((currentPage - 1) * productsPerPage).Take(productsPerPage);
            var result = new Tuple<List<ProductDTO>, int>(items.ToList(), list.Count());
            return result;
        }

        public int addProduct(ProductDTO product)
        {
            return _productDAL.create(product);
        }

        public int updateProduct(ProductDTO product)
        {
            return _productDAL.update(product);
        }

        public int deleteProduct (int id)
        {
            return _productDAL.delete(id);
        }

        public string uploadImage(FileInfo selectedImage, int id, string token)
        {
            _productDAL.updateThumbnail(id, token);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = $"{folder}/Assets/Images/Product/{token}.png";
            var relativePath = $"Assets/Images/Product/{token}.png";
            File.Copy(selectedImage.FullName, filePath);

            return relativePath;
        }
    }
}
