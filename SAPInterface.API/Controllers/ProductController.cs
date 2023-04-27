using Microsoft.AspNetCore.Mvc;
using SApInterface.API.Repositry;
using System.Reflection;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using SApInterface.API.Model.Domain;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Text;
using System;
using System.Xml;
using Microsoft.AspNetCore.Diagnostics;
using System.Runtime.Serialization;

namespace SApInterface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepositry productRepository;
    

        public ProductController(IProductRepositry productRepository)
        {
            this.productRepository = productRepository;
        
        }


        //[HttpGet]

        //public async Task<IActionResult> GetAllSections()
        //{
        //    var section = await productRepository.GetAsync();
                      
        //    return Ok(section);

        //}
       
      

        [HttpGet]
        public async Task<IActionResult> GetSectionAsync()
        {
              Product prod = new Product ();
            string lstrErr;
            DataSet Mdsdataset = new DataSet();
            DataTable LDTWorkListPerm = Mdsdataset.Tables.Add("Product");
            LDTWorkListPerm.Columns.Add("PRODCODE", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("PRODSHTNAME", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("PRODLNGNAME", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("PRODCATECODE", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("REGISTRATIONNO", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("BATCHSIZE", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("STATUS", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("STORAGECON", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("UNIT", Type.GetType("System.String"));
            LDTWorkListPerm.Columns.Add("Status", Type.GetType("System.String"));

            DataRow LRow;
            Boolean blninsert = false;

            if (prod == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                string userPassword = "api_lims:Sap@123456";
                string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPassword));

                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);

                // Setting Base address.  
                 
                // Setting Header key.  
                    client.DefaultRequestHeaders.Add("X-ApiKey", "MyRandomApiKeyValue");
              

                var url = "http://172.18.4.35:8000//sap/opu/odata/sap/API_PRODUCT_SRV/A_Product";



                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);



                    string xmlString = xdoc.ToString();

                  
                    XmlDocument doc = new XmlDocument();

          
                    doc.LoadXml(xmlString);

                    // Find the root element of the feed
                    XmlElement root = doc.DocumentElement;

                    // Get the namespace manager for the feed
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");

                    // Parse the feed entries
                    //XmlNodeList entries = root.SelectNodes("//atom:entry", nsmgr);
                    //foreach (XmlNode entry in entries)
                    //{
                    //    // Get the title and link of the entry
                    //    XmlNode titleNode = entry.SelectSingleNode("atom:title", nsmgr);
                    //    string title = titleNode.InnerText;

                    //    XmlNode linkNode = entry.SelectSingleNode("atom:link[@rel='http://schemas.microsoft.com/ado/2007/08/dataservices/related/to_Valuation']", nsmgr);
                    //    string link = linkNode.Attributes["href"].Value;

                    //    XmlNode contentnode = entry.SelectSingleNode("atom:content", nsmgr);
                    //    string content = titleNode.InnerText;

                    //    // Do something with the title and link
                    //    Console.WriteLine("Title: {0}", title);
                    //    Console.WriteLine("Link: {0}", link);
                    //}
                   

                    XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                   nsManager.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                    XmlNodeList conentries = root.SelectNodes("//atom:content", nsmgr);
                    foreach (XmlNode entry in conentries)
                    {
                        //Fetch single the Nodes.
                       // XmlNodeList nodes = doc.SelectNodes("//d:Product", nsManager);

                        //Fetch all the Nodes.
                        XmlNodeList nodes = entry.SelectNodes("//text()");

                       
                        foreach (XmlNode node in nodes)
                        {

                            Console.WriteLine(node.ParentNode.Name + ": " + node.InnerText);
                                                     

                            if (node.ParentNode.Name=="d:Product")
                            {
                              
                               prod.ItemCode = node.InnerText.Trim();
                            }
                            else if(node.ParentNode.Name == "d:ProductType")
                            {

                                prod.ProductCategory = node.InnerText.Trim();
                               
                            }
                            else if (node.ParentNode.Name == "d:BaseUnit")
                            {

                                prod.Unit = node.InnerText.Trim();
                                blninsert = true;


                            }
                            if (blninsert==true)
                            {
                                LRow = Mdsdataset.Tables["Product"].NewRow();
                                LRow["PRODCODE"] = prod.ItemCode.Trim();
                                LRow["PRODCATECODE"] = prod.ProductCategory.Trim();
                                LRow["UNIT"] = prod.Unit.Trim();
                                LRow["Status"] = "Y";
                                Mdsdataset.Tables["Product"].Rows.Add(LRow);
                                Mdsdataset.Tables["Product"].AcceptChanges();
                                blninsert= false;
                            }
                           

                        }


                        //XmlNode prodcodeNode = doc.SelectSingleNode("//d:Product", nsManager);
                        //XmlNode prodcateNode = doc.SelectSingleNode("//d:ProductType", nsManager);

                        //LRow = Mdsdataset.Tables["Product"].NewRow();
                        //LRow["PRODCODE"] = prodcodeNode.InnerText.Trim();
                        //LRow["PRODCATECODE"] = prodcateNode.InnerText.Trim();
                        //Mdsdataset.Tables["Product"].Rows.Add(LRow);
                        //Mdsdataset.Tables["Product"].AcceptChanges();



                        break;

                   }

                    prod.ds = Mdsdataset;


                }
                   
                }

            using (var client = new HttpClient())
            {
                string userPassword = "api_lims:Sap@123456";
                string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPassword));
                           
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                client.DefaultRequestHeaders.Add("X-ApiKey", "MyRandomApiKeyValue");


                var url = "http://172.18.4.35:8000//sap/opu/odata/sap/API_PRODUCT_SRV/A_ProductDescription";


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    string xmlString = xdoc.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlElement root = doc.DocumentElement;

                  
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");

                  
                  

                    XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                    nsManager.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                    XmlNodeList conentries = root.SelectNodes("//atom:content", nsmgr);
                   
                        
                        XmlNodeList nodes = doc.SelectNodes("//text()");


                        foreach (XmlNode node in nodes)
                        {

                            Console.WriteLine(node.ParentNode.Name + ": " + node.InnerText);


                            if (node.ParentNode.Name == "d:Product")
                            {

                                prod.ItemCode = node.InnerText.Trim();
                            }
                            else if (node.ParentNode.Name == "d:ProductDescription")
                            {

                                prod.ShortName = node.InnerText.Trim();
                                blninsert = true;
                            }
                            DataRow[] Slow;
                            if (blninsert == true)
                            {
                                Slow = Mdsdataset.Tables["Product"].Select("PRODCODE='" + prod.ItemCode.Trim() + "'");
                                foreach (var row in Slow)
                                {
                                    row["PRODSHTNAME"] = prod.ShortName.Trim();
                                    Mdsdataset.Tables["Product"].AcceptChanges();
                                }
                             
                                blninsert = false;
                            }


                        }


                    prod.ds = Mdsdataset;


                }

            }

            using (var client = new HttpClient())
            {
                string userPassword = "api_lims:Sap@123456";
                string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPassword));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                client.DefaultRequestHeaders.Add("X-ApiKey", "MyRandomApiKeyValue");


                var url = "http://172.18.4.35:8000//sap/opu/odata/sap/API_PRODUCT_SRV/A_ProductInspectionText";


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    string xmlString = xdoc.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlElement root = doc.DocumentElement;


                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");




                    XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                    nsManager.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                    XmlNodeList conentries = root.SelectNodes("//atom:content", nsmgr);


                    XmlNodeList nodes = doc.SelectNodes("//text()");


                    foreach (XmlNode node in nodes)
                    {

                        Console.WriteLine(node.ParentNode.Name + ": " + node.InnerText);


                        if (node.ParentNode.Name == "d:Product")
                        {

                            prod.ItemCode = node.InnerText.Trim();
                        }
                        else if (node.ParentNode.Name == "d:LongText")
                        {

                            prod.LongName = node.InnerText.Trim();
                            blninsert = true;
                        }
                        DataRow[] Slow;
                        if (blninsert == true)
                        {
                            Slow = Mdsdataset.Tables["Product"].Select("PRODCODE='" + prod.ItemCode.Trim() + "'");
                            foreach (var row in Slow)
                            {
                                row["PRODLNGNAME"] = prod.LongName.Trim();
                                Mdsdataset.Tables["Product"].AcceptChanges();
                            }

                            blninsert = false;
                        }


                    }


                    prod.ds = Mdsdataset;


                }

            }

            using (var client = new HttpClient())
            {
                string batchsize1, batchsize2;
                string userPassword = "api_lims:Sap@123456";
                string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPassword));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                client.DefaultRequestHeaders.Add("X-ApiKey", "MyRandomApiKeyValue");


                var url = "http://172.18.4.35:8000//sap/opu/odata/sap/API_PRODUCT_SRV/A_ProductPlant";


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    string xmlString = xdoc.ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlElement root = doc.DocumentElement;


                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");




                    XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                    nsManager.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
                    XmlNodeList conentries = root.SelectNodes("//atom:content", nsmgr);


                    XmlNodeList nodes = doc.SelectNodes("//text()");
                    batchsize1 = string.Empty;
                    batchsize2 = string.Empty;

                    foreach (XmlNode node in nodes)
                    {

                        Console.WriteLine(node.ParentNode.Name + ": " + node.InnerText);
                       

                        if (node.ParentNode.Name == "d:Product")
                        {

                            prod.ItemCode = node.InnerText.Trim();
                        }
                        else if (node.ParentNode.Name == "d:MinimumLotSizeQuantity")
                        {

                            prod.BatchSize = node.InnerText.Trim();
                           
                        }
                        else if (node.ParentNode.Name == "d:MaximumLotSizeQuantity")
                        {

                           batchsize1 = node.InnerText.Trim();
                         
                        }
                        else if (node.ParentNode.Name == "d:FixedLotSizeQuantity")
                        {

                           batchsize2 = node.InnerText.Trim();
                            blninsert = true;
                        }
                        DataRow[] Slow;
                        if (blninsert == true)
                        {
                            Slow = Mdsdataset.Tables["Product"].Select("PRODCODE='" + prod.ItemCode.Trim() + "'");
                            foreach (var row in Slow)
                            {
                                row["BATCHSIZE"] = prod.BatchSize.Trim() + string.Concat("-"  , batchsize1 , "-",   batchsize2);
                                Mdsdataset.Tables["Product"].AcceptChanges();
                            }

                            blninsert = false;
                        }


                    }


                    prod.ds = Mdsdataset;


                }

            }

           lstrErr= productRepository.AddAsync(prod, Mdsdataset);

            return Ok(prod);
        }




    }
}
