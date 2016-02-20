using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;

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
            return "index.html";
        }

        // GET api/values/5
        [HttpGet("{code}")]
        public string Get(string code)
        {
            return code;
        }

        // POST api/values
        [HttpPost]
        public FileResult Post(string code)//parameter name code
        {
            //string dir = _appEnvironment.ApplicationBasePath;
            //byte[] fileBytes = System.IO.File.ReadAllBytes(dir + @"\wwwroot\placeholder.jpg");
            //string fileName = "downloaded.jpg";
            //return File(fileBytes, "application/octet-stream", fileName);
            //return code;
            string dir = _appEnvironment.ApplicationBasePath + "\\wwwroot\\Asp5WebApiTemplate";
            var baseOutputStream = new MemoryStream();
            ZipOutputStream zip = new ZipOutputStream(baseOutputStream);
            zip.IsStreamOwner = false;
            zip.SetLevel(3);
            ZipFolder(dir, dir, zip);
            //byte[] buffer = new byte[4096];
            //foreach (var file in Directory.GetFiles(dir))
            //{
            //    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
            //    entry.DateTime = DateTime.Now;
            //    zip.PutNextEntry(entry);

            //    using (FileStream fs = System.IO.File.OpenRead(file))
            //    {
            //        int sourceBytes = 0;
            //        do
            //        {
            //            sourceBytes = fs.Read(buffer, 0, buffer.Length);
            //            zip.Write(buffer, 0, sourceBytes);
            //        } while (sourceBytes > 0);
            //    }
            //}

            //ZipFile zf = new ZipFile(baseOutputStream);
            //zf.BeginUpdate();
            ////create the code file
            
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(code));
            //TextWriter tw = new StreamWriter(ms);
            //tw.WriteLine(code);
            //tw.Flush();

            byte[] buffer = new byte[4096];
            string fileRelativePath = "\\Controllers/code.txt";

            ZipEntry entry = new ZipEntry(fileRelativePath);
            entry.DateTime = DateTime.Now;
            zip.PutNextEntry(entry);
            
            int sourceBytes;
            do
            {
                sourceBytes = ms.Read(buffer, 0, buffer.Length);
                zip.Write(buffer, 0, sourceBytes);
            } while (sourceBytes > 0);
            zip.Finish();
            
            //FileContentResult f = File(ms.GetBuffer(), "text/plain", "file.txt");
            //CustomStaticDataSource csds = new CustomStaticDataSource();
            //csds.SetStream(ms);
            //zf.Add(csds, "code.txt");
            //zf.CommitUpdate();
            //zf.IsStreamOwner = false;
            //zf.Close();

            baseOutputStream.Position = 0;
            return new FileStreamResult(baseOutputStream, "application/x-zip-compressed")
            {
                FileDownloadName = "asdf.zip"
            };
        }

        private void ZipFolder(string RootFolder, string CurrentFolder,
    ZipOutputStream zStream)
        {
            string[] SubFolders = Directory.GetDirectories(CurrentFolder);

            //calls the method recursively for each subfolder
            foreach (string Folder in SubFolders)
            {
                ZipFolder(RootFolder, Folder, zStream);
            }

            string relativePath = CurrentFolder.Substring(RootFolder.Length) + "/";

            //the path "/" is not added or a folder will be created
            //at the root of the file
            if (relativePath.Length > 1)
            {
                ZipEntry dirEntry;
                dirEntry = new ZipEntry(relativePath);
                dirEntry.DateTime = DateTime.Now;
            }

            //adds all the files in the folder to the zip
            foreach (string file in Directory.GetFiles(CurrentFolder))
            {
                AddFileToZip(zStream, relativePath, file);
            }
        }

        private void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];

            //the relative path is added to the file in order to place the file within
            //this directory in the zip
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                      + Path.GetFileName(file);

            ZipEntry entry = new ZipEntry(fileRelativePath);
            entry.DateTime = DateTime.Now;
            zStream.PutNextEntry(entry);

            using (FileStream fs = System.IO.File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
        private class CustomStaticDataSource : IStaticDataSource
        {
            private Stream _stream;
            // Implement method from IStaticDataSource
            public Stream GetSource()
            {
                return _stream;
            }

            // Call this to provide the memorystream
            public void SetStream(Stream inputStream)
            {
                _stream = inputStream;
                _stream.Position = 0;
            }
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
