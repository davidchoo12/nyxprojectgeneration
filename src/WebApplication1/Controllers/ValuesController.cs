using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IApplicationEnvironment _appEnvironment;

        public ValuesController(IApplicationEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return _appEnvironment.ApplicationBasePath;
            //byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\Development\asdf.txt");
            //string fileName = "myfile.txt";
            //return File(fileBytes, "application/octet-stream", fileName);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //var dir = ("/Images");
            //var path = Path.Combine(dir, id + ".jpg");
            //return base.File(path, "image/jpeg");
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
