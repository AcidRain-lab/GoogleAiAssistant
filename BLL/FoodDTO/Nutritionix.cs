using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.FoodDTO
{
    public class NutritionixRequest
    {
        public string Query { get; set; }
    }

    public class NutritionixResponse
    {
        public string FoodName { get; set; }
        public float ServingQty { get; set; }
        public string ServingUnit { get; set; }
        public float ServingWeightGrams { get; set; }
        public float Calories { get; set; }
    }


}
