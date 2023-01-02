using EEH.RestAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEH.Ecommerce
{
    public interface IOpenApi
    {
        Task<int> GetProductTotalCnt(string keyword);
    }
}
