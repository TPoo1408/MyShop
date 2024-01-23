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
    public class PromotionBLL
    {
        private PromotionDAL _promotionDAL;

        public PromotionBLL()
        {
            _promotionDAL = new PromotionDAL();
        }

        public PromotionDTO getPromotionById (int id)
        {
            return _promotionDAL.getById(id);
        }

        public ObservableCollection<PromotionDTO> getAllPromotion()
        {
            var result = _promotionDAL.getAll();
            return result;
        }

        public int addPromotion(PromotionDTO promotion)
        {
            return _promotionDAL.create(promotion);
        }

        public int updatePromotion(PromotionDTO promotion)
        {
            return _promotionDAL.update(promotion);
        }

        public int deletePromotion(int id)
        {
            return _promotionDAL.delete(id);
        }

       
    }
}
