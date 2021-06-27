using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<NexonAccount> Get()
        {
            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var files = di.GetFiles("*.nxmbng");

            return files
                .Select(e =>
                {
                    Console.WriteLine(e.FullName);

                    return JsonConvert.DeserializeObject<NexonAccount>(e.FullName);
                })
                .ToArray();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var fi = new FileInfo(di.FullName + $"/{id}.nxmbng");
            return JsonConvert.SerializeObject(fi);
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] CreateNexonAccount value)
        {
            if (value == null) return;

            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var files = di.GetFiles("*.nxmbng").OrderBy(e=>e.Name).LastOrDefault();

            long uid = 0;
            if (files != null && files.Exists)
            {
                var lastId = long.Parse(files.Name.Replace(".nxmbng", ""));
                uid = lastId + 1;
            }

            var acc = new NexonAccount()
            {
                UID = uid,

                AccountType = value.AccountType,
                Email = value.Email,
                Password = value.Password,
                DisplayName = value.DisplayName,

                Description = value.Description,

                CreationTime = DateTime.Now,
                LastModifiedTime = DateTime.Now,
                LastSignedInTime = DateTime.Now,
            };

            System.IO.File.WriteAllText($"accounts/{acc.UID}.nxmbng", JsonConvert.SerializeObject(acc));
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
