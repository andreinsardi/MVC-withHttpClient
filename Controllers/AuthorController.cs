using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_withHttpClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC_withHttpClient.Controllers
{
    public class AuthorController : Controller
    {
        private readonly Appsettings _appSettings;

        public AuthorController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(_appSettings.BlogDataAPI + "/api/Author");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authors = JsonConvert.DeserializeObject<List<AuthorViewModel>>(content);

                ViewData["authors"] = authors;

                return View();
            }
            else
            {
                ViewData["authors"] = new List<AuthorViewModel>();
                return View();
            }



        }

        public async Task<IActionResult> Create(int AuthorID)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(_appSettings.BlogDataAPI + "/api/Author/" + AuthorID);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var authorViewModel = JsonConvert.DeserializeObject<AuthorViewModel>(content);


                return View(authorViewModel);
            }
            else
            {
                return View(new AuthorViewModel { });
            }


        }

        public async Task<IActionResult> Delete(int AuthorID)
        {
            HttpClient client = new HttpClient();

            var response = await client.DeleteAsync(_appSettings.BlogDataAPI + "/api/Author/" + AuthorID);
            var content = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Add(AuthorViewModel request)
        {

            HttpClient client = new HttpClient();

        

            if (request.AuthorID > 0)
            {

                var response = await client.PutAsJsonAsync(_appSettings.BlogDataAPI + "/api/Author/" + request.AuthorID, new StringContent(JsonConvert.SerializeObject(request,
                                                                                                new JsonSerializerSettings
                                                                                                {
                                                                                                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                                }), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();




            }
            else
            {
                var response = await client.PostAsync(_appSettings.BlogDataAPI + "/api/Author/" , new StringContent(JsonConvert.SerializeObject(request,
                                                                                              new JsonSerializerSettings
                                                                                              {
                                                                                                  ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                                                              }), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();

            } 

            return RedirectToAction("Index");
        }
    }
}
