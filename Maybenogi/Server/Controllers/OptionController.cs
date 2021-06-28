using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Maybenogi.Server.Module;
using Maybenogi.Shared.Model;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace Maybenogi.Server.Controllers
{
    [EnableCors(Constant.CORS)]
    [Route("api/options")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        [HttpGet]
        public async Task<Option> Get()
        {
            var di = new DirectoryInfo("settings");
            if (!di.Exists)
                di.Create();

            Option option;
            var fi = new FileInfo("settings/setting.json");
            if (!fi.Exists)
            {
                option = new Option();
                var json = JsonConvert.SerializeObject(option);

                await System.IO.File.WriteAllTextAsync(fi.FullName, json);
            }
            else
            {
                var json = System.IO.File.ReadAllTextAsync(fi.FullName);
                option = JsonConvert.DeserializeObject<Option>(json.Result);
            }

            return option;
        }

        [HttpPost]
        public async Task Post([FromBody] Option option)
        {
            var di = new DirectoryInfo("settings");
            if (!di.Exists)
                di.Create();

            var json = JsonConvert.SerializeObject(option);
            await System.IO.File.WriteAllTextAsync("settings/setting.json", json);

            SeleniumHandler.Instance.SetOption(option);
        }
    }
}
