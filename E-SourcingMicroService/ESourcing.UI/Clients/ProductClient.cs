using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class ProductClient
    {
        public HttpClient client { get; }

        public ProductClient(HttpClient httpClient)
        {
            client = httpClient;
            client.BaseAddress = new Uri(CommonInfo.BaseAddress);   
        }

        public async Task<Result<List<ProductViewModel>>> GetProductsAsync()
        {
            var response = await client.GetAsync("/Product");
            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductViewModel>>(responseData);

                if (data.Any())
                {
                    return new Result<List<ProductViewModel>>(true, ResultConstant.RecordFound, data.ToList());
                }
            }
            return new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);

        }
    }
}
