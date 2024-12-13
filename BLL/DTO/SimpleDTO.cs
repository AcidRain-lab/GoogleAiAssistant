using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{

    public class JobCategoryDTO : SimpleDTO
    {

    }
    public class PhoneTypeDTO : SimpleDTO
    {

    }
    public class SimpleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
