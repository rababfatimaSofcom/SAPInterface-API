using Microsoft.AspNetCore.Mvc;
using SApInterface.API.Repositry;
using System.Reflection;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using SApInterface.API.Model.Domain;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SApInterface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly ISectionRepositry sectionRepository;
    

        public ProductController(ISectionRepositry sectionRepository)
        {
            this.sectionRepository = sectionRepository;
        
        }


        [HttpGet]

        public async Task<IActionResult> GetAllSections()
        {
            var section = await sectionRepository.GetAsync();
                      
            return Ok(section);

        }
       
      

        [HttpGet]
        [Route("{id}")]
         public async Task<IActionResult> GetSectionAsync(string id)
        {
              Product prod = new Product ();

            if (prod == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7131/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = client.GetAsync("Section");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    //readTask.Wait();
                    var jsonString = result.Content.ReadAsStringAsync().Result;
                    DataTable dtStdMarks = (DataTable)JsonConvert.DeserializeObject(jsonString, typeof(DataTable));


                    prod.ItemCode = dtStdMarks.Rows[0]["sectionCode"].ToString();
                    prod.ShortName = dtStdMarks.Rows[0]["ProductDescription"].ToString();

                    prod.LongName = dtStdMarks.Rows[0]["sectionName"].ToString();
                    prod.BatchSize = dtStdMarks.Rows[0]["sectionName"].ToString();
                    prod.Leadtime = double.Parse(dtStdMarks.Rows[0]["sectionName"].ToString());
                    prod.Unit = dtStdMarks.Rows[0]["BaseUnit"].ToString();
                    prod.StorageCondition = dtStdMarks.Rows[0]["sectionName"].ToString();
                    prod.RegistrationNo = dtStdMarks.Rows[0]["sectionName"].ToString();
                    //txtstdmarks.Text = dtStdMarks.Rows[0]["MarksObtained"].ToString();
                    //txttestdate.Text = dtStdMarks.Rows[0]["TestDate"].ToString();

                }
                else //web api sent error response 
                {
                    //log response status here..

                    //txtstdname.Text = string.Empty;
                    //txtcrsname.Text = string.Empty;
                    //txtstdmarks.Text = string.Empty;
                    //txttestdate.Text = string.Empty;

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return Ok(prod);
        }




    }
}
