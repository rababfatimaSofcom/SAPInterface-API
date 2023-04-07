using Microsoft.AspNetCore.Mvc;
using SApInterface.API.Repositry;
using System.Reflection;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using SApInterface.API.Model.Domain;
using Newtonsoft.Json;

namespace SApInterface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class SectionController : Controller
    {
        private readonly ISectionRepositry sectionRepository;
    

        public SectionController(ISectionRepositry sectionRepository)
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
            var section = await sectionRepository.GetSectionAsync(id);

            if (section == null)
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


                   section.sectionCode = dtStdMarks.Rows[0]["sectionCode"].ToString();
                    section.sectionName = dtStdMarks.Rows[0]["sectionName"].ToString();
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
            return Ok(section);
        }




    }
}
