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
    public class AuctionClient
    {
        public HttpClient client { get; set; }

        public AuctionClient(HttpClient _client)
        {
            client = _client;
            client.BaseAddress = new Uri(CommonInfo.BaseAddress);

        }

        public async Task<Result<AuctionViewModel>> CreateAuctionAsync(AuctionViewModel model)
        {
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("/Auction", content);

            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);

                if (result != null)
                    return new Result<AuctionViewModel>(true, ResultConstant.RecordCreateSuccessfully, result);
            }
            return new Result<AuctionViewModel>(false, ResultConstant.RecordCreateNotSuccessfully);
        }

        public async Task<Result<List<AuctionViewModel>>> GetAuctionsAsync()
        {
            var response = await client.GetAsync("/Auction");

            if (response.IsSuccessStatusCode)
            {
                var responseData =await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<AuctionViewModel>>(responseData);

                if (result.Any())
                    return new Result<List<AuctionViewModel>>(true, ResultConstant.RecordFound, result.ToList());
            }

            return new Result<List<AuctionViewModel>>(false, ResultConstant.RecordNotFound);

        }

        public async Task<Result<AuctionBidsViewModel>> GetAuctionByIdAsync(string id)
        {
            var response = await client.GetAsync("/Auction/" + id);
            if(response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuctionBidsViewModel>(responseData);
                
                if (result != null)
                    return new Result<AuctionBidsViewModel>(true, ResultConstant.RecordFound, result);
                
            }

            return new Result<AuctionBidsViewModel>(false, ResultConstant.RecordNotFound);

        }

        public async Task<Result<string>> CompleteAuction(string id)
        {
            var dataAsString = JsonConvert.SerializeObject(id);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType =new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("/Auction/CompleteAuction", content);

            if(response.IsSuccessStatusCode)
            {
                return new Result<string>(true, ResultConstant.RecordCompleted);
            }
            return new Result<string>(false, ResultConstant.RecordNotCompleted);

        }
    }
}
