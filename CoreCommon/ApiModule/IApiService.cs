using CoreCommon.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon
{
   public interface IApiService
    {
        Task<ResponseApi<T>> getAsync<T>(RequestApi api) where T : class;

        Task<ResponseApi<T>> postAsync<T>(RequestApi api) where T : class;
    }
}
