using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class BidClient
    {
        public HttpClient client;

        public BidClient(HttpClient httpClient)
        {
           client = httpClient;
            client.BaseAddress = new Uri(CommonInfo.BaseAddress);
        }

        public async Task<Result<List<BidViewModel>>> GetAllBidByAuctionIdAsync(string auctionId)
        {
            var response = await client.GetAsync($"/Bid/GetBidsByAuctionId/{auctionId}");

            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<BidViewModel>>(responseData);
                if(result.Any())
                    return new Result<List<BidViewModel>>(true, ResultConstant.RecordFound,result.ToList());

            }

            return new Result<List<BidViewModel>>(false, ResultConstant.RecordNotFound,new List<BidViewModel>());
        }

        public async Task<Result<string>> SendBid(BidViewModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("/Bid", content);

            if(response.IsSuccessStatusCode)
            {
                 return new Result<string>(true, ResultConstant.RecordFound);
            }
            return new Result<string>(false, ResultConstant.RecordNotFound);

        }
    }
}
