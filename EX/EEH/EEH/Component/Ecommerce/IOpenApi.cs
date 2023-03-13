using EEH.RestAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EEH.Component.Ecommerce
{
    public interface IOpenApi
    {
        EcommerceType Type { get; }
        Task<int> GetProductTotalCnt(string keyword);
    }
}
