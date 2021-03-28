using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLib.Models;
using ApplicationLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SalesLib.Models;

namespace SalesWeb.Controllers
{
    public class OrderController : Controller
    {
        public IConfiguration _Configurations { get; set; }

        public OrderController(IConfiguration configurations)
        {
            _Configurations = configurations;
        }

        public async Task<ActionResult> Index(string Search="")
        {
            var baseUri = _Configurations["AppSettings:SalesApi:baseUri"].ToString();
            var getAllUri = _Configurations["AppSettings:SalesApi:GetAll"].ToString();
            var uri = ApiClientService.CreateRequestUri(baseUri, getAllUri.Replace("{search}", Search), "");
            var orders = await ApiClientService.GetAsync<IEnumerable<Order>>(uri);
            return View(orders);
        }

        public ActionResult Create()
        {
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var baseUri = _Configurations["AppSettings:SalesApi:baseUri"].ToString();
                    var createUri = _Configurations["AppSettings:SalesApi:Create"].ToString();
                    var uri = ApiClientService.CreateRequestUri(baseUri, createUri, "");
                    var response = await ApiClientService.PostAsync<Order, MessageResponseModel<string>>(uri, model);
                    response.Message = "Order has been added successfully.";
                    if (response.Success)
                        return RedirectToAction(nameof(Index), nameof(Order));
                }
                else
                {
                    TempData["ErrorMessage"] = "Check the order values";
                }
                //else
                //{
                //    response.ValidationList = ModelStateBase.GetValidationErrors(ModelState);
                //}
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(new Order());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var baseUri = _Configurations["AppSettings:SalesApi:baseUri"].ToString();
                var createUri = _Configurations["AppSettings:SalesApi:Get"].ToString();
                var uri = ApiClientService.CreateRequestUri(baseUri, createUri.Replace("{id}", id.ToString()));
                var order = await ApiClientService.GetAsync<Order>(uri);
                return View(order);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var baseUri = _Configurations["AppSettings:SalesApi:baseUri"].ToString();
                    var updateUri = _Configurations["AppSettings:SalesApi:Update"].ToString();
                    var uri = ApiClientService.CreateRequestUri(baseUri, updateUri, "");
                    var response = await ApiClientService.PostAsync<Order, MessageResponseModel<string>>(uri, model);
                    response.Message = "Order has been updated successfully.";
                    if (response.Success)
                        return RedirectToAction(nameof(Index), nameof(Order));
                }
                else
                {
                    TempData["ErrorMessage"] = "Check the order values";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var baseUri = _Configurations["AppSettings:SalesApi:baseUri"].ToString();
                var deleteUri = _Configurations["AppSettings:SalesApi:Delete"].ToString();
                var uri = ApiClientService.CreateRequestUri(baseUri, deleteUri, "");
                var response = await ApiClientService.PostAsync<int, MessageResponseModel<string>>(uri, id);
                response.Message = "Order has been deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), nameof(Order));
        }
    }
}
