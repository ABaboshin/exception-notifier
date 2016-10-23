using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotifierShared.Config;
using NotifierShared.Logic;
using RestListener.Models;
using StackExchange.Redis;

namespace RestListener.Controllers {
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IOptions<RedisOptions> _optionsAccessor;

        public NotificationController(IOptions<RedisOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Notification item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            ConnectionMultiplexer redis = null;
            try {
                redis = ConnectionMultiplexer.Connect(NetHelper.ResolceNameToIp(_optionsAccessor.Value.RedisHost));
                item.Id = Guid.NewGuid().ToString();
                var db = redis.GetDatabase();
                var result = db.SortedSetAdd(
                    _optionsAccessor.Value.ActiveSet,
                    JsonConvert.SerializeObject(item),
                    DateTime.UtcNow.Ticks
                );
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return BadRequest(ex);
            } finally {
                if (redis != null)
                {
                    redis.Close();
                }
            }
            
            return Json("ok");
        }
    }
}
