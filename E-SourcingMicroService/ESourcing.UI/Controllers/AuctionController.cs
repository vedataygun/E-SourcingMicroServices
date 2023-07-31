using ESourcing.Core.ResultModels;
using ESourcing.Infrastructure.Repositories.Base;
using ESourcing.UI.Clients;
using ESourcing.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class AuctionController : Controller
    {

        readonly IUserRepository _userRepository;
        readonly ProductClient _productClient;
        readonly AuctionClient _auctionClient;
        readonly BidClient _bidClient;

        public AuctionController(IUserRepository userRepository ,ProductClient productClient ,AuctionClient auctionClient, BidClient bidClient)
        {
            _userRepository = userRepository;
            _productClient = productClient;
            _auctionClient = auctionClient;
            _bidClient = bidClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _auctionClient.GetAuctionsAsync();

            if(result.IsSuccess)
            {
                return View(result.Data);
            }

            return View(new List<AuctionViewModel>());
        }

        public async Task<IActionResult> Create()
        {

            //TODO:Product GetAll
            var productList = await _productClient.GetProductsAsync();
            if(productList.IsSuccess)
            {
                ViewBag.ProductList = productList.Data;
            }

            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            model.Status = 0;
            model.CreatedAt = DateTime.Now;

            var createAuction = await _auctionClient.CreateAuctionAsync(model);
             if(createAuction.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Detail(string id)
        {
            AuctionBidsViewModel model = new AuctionBidsViewModel();

            
            var auction = await _auctionClient.GetAuctionByIdAsync(id);
            var bidList = await _bidClient.GetAllBidByAuctionIdAsync(id);


            model.SellerUserName = HttpContext.User?.Identity.Name;
            model.Email = HttpContext.User?.FindFirstValue(ClaimTypes.Email);
            model.ID = auction.Data.ID;
            model.ProductId = auction.Data.ProductId;
            model.Bids = bidList.Data;
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            model.isAdmin = Convert.ToBoolean(isAdmin);


            return View(model);
        }

        [HttpPost]
        public async Task<Result<string>> SendBid(BidViewModel model)
        {
            model.CreatedAt = DateTime.Now;
            var sendBidResponse = await _bidClient.SendBid(model);

            return sendBidResponse;
        }

        [HttpPost]
        public async Task<Result<string>> CompleteBid(BidViewModel model)
        {
            var completeBidResponse = await _auctionClient.CompleteAuction(model.AuctionId);

            return completeBidResponse;
        }
    }
}
