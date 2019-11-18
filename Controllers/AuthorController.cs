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
using MySql.Data.MySqlClient;

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
        public IActionResult Index()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            List<AuthorViewModel> authors = new List<AuthorViewModel>();
            List<PostViewModel> posts = new List<PostViewModel>();


            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT Author.AuthorID, Author.Name FROM Author", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        authors.Add(new AuthorViewModel
                        {
                            AuthorID = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1)
                        });

                    }
                }

                ViewData["authors"] = authors;

                return View();
            }
            catch (Exception ex)
            {
                return View(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }       

        }

        public IActionResult Create(int AuthorID)
        {
           
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AuthorViewModel authorViewModel = new AuthorViewModel();


            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AuthorID, Name FROM Author where AuthorID =" + AuthorID, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {


                        authorViewModel.AuthorID = dataReader.GetInt32(0);
                        authorViewModel.Name = dataReader.GetString(1);
                        

                    }
                }

                if (authorViewModel.AuthorID>0)
                {
                    return View(authorViewModel);
                }
                else
                {
                    return View(new AuthorViewModel { });
                }

            }
            catch (Exception ex)
            {
                return View(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        public IActionResult Delete(int AuthorID)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try {

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Author WHERE AuthorID =" + AuthorID, conn))
                {

                    cmd.ExecuteNonQuery();


                }

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        [HttpPost]
        public IActionResult Add(AuthorViewModel request)
        {
          
          MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                if (request.AuthorID > 0)
                {

                    using (MySqlCommand cmd = new MySqlCommand("update Author set name = @name where AuthorID = @AuthorId", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", request.Name);
                        cmd.Parameters.AddWithValue("@AuthorId", request.AuthorID);

                        cmd.ExecuteNonQuery();
                    }


                }
                else
                {
                    using (MySqlCommand cmd = new MySqlCommand("insert into Author (Name) Values (@name)", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", request.Name);

                        cmd.ExecuteNonQuery();


                    }
                }

                return RedirectToAction("Index");
             }
            catch (Exception ex)
            {
                return View(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
    }
}
