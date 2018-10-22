using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Interfaces
{
    public interface IDimensionsService : IDisposable
    {
        /// <summary>
        /// Return if the User has permission to use the Dimension (DimensionTag) in the Product.
        /// </summary>
        /// <param name="BaseParam"></param>
        /// <param name="DimensionTag"></param>
        /// <returns></returns>
        bool GetSwitchDimension(string ProductToken, string UserCode, string DimensionTag);

        /// <summary>
        /// Return the current value of the dimensionhe anon user.
        /// </summary>
        /// <param name="ProductToken"></param>
        /// <param name="DimensionTag"></param>
        /// <returns></returns>
        bool GetAnonSwitchDimension(int IdProduct,int IdDimension);

        /// <summary>
        /// Return current value of the dimension in the user wallet for this specific Product.
        /// </summary>
        /// <param name="BaseParam"></param>
        /// <param name="DimensionTag"></param>
        /// <returns></returns>
        decimal GetNumericDimension(string ProductToken, string UserCode, string DimensionTag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductToken"></param>
        /// <param name="UserCode"></param>
        /// <param name="DimensionTag"></param>
        /// <returns></returns>
        decimal GetNumericConsumableDimension(string ProductToken, string UserCode, string DimensionTag);

        /// <summary>
        /// Consumes the indicated quantity of the indicated dimension of the user wallet for this product
        /// </summary>
        /// <param name="BaseParam"></param>
        /// <param name="DimensionTag"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        decimal ConsumeNumericDimension(string ProductToken, string UserCode, string DimensionTag, decimal Amount);
        decimal ConsumeNumericConsumableDimension(string productToken, string userCode, string dimensionTag, decimal amount);
    }
}
