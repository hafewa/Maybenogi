using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maybenogi.Shared;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route(ApiRoute.ACCOUNT)]
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
            if (files.Length < 1) return new NexonAccount[0];

            return files
                .Select(e =>
                {
                    Console.WriteLine(e.FullName);

                    var deserialized =
                        JsonConvert.DeserializeObject<NexonAccount>(System.IO.File.ReadAllText(e.FullName));

                    return deserialized;
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

            var fi = new FileInfo(di.FullName + $"/{id:00000000}.nxmbng");
            return System.IO.File.ReadAllText(fi.FullName);
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] CreateNexonAccount value)
        {
            if (value == null) return;

            var di = new DirectoryInfo("accounts");
            if (!di.Exists)
                di.Create();

            var files = di.GetFiles("*.nxmbng")
                .OrderBy(e=>e.Name)
                .LastOrDefault();

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

            System.IO.File.WriteAllText($"accounts/{ConvertFromId(acc.UID)}.nxmbng", JsonConvert.SerializeObject(acc));
        }

        [HttpPost("edit")]
        public void Edit([FromBody] EditNexonAccount account)
        {
            if (account == null) return;

            var nexonAccJson = Get((int)account.UID);
            var acc = JsonConvert.DeserializeObject<NexonAccount>(nexonAccJson);
            acc.Import(account);
            
            System.IO.File.WriteAllText($"accounts/{ConvertFromId(acc.UID)}.nxmbng", JsonConvert.SerializeObject(acc));
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

        private string ConvertFromId(long uid)
        {
            return $"{uid:00000000}";
        }
    }
}
