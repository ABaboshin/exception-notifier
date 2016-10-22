using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotifierShared.Config;
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

            try {
                var ip = System.Net.Dns.GetHostEntryAsync(_optionsAccessor.Value.RedisHost).Result;
                var redis = ConnectionMultiplexer.Connect(ip.AddressList[0].ToString());
                //if (string.IsNullOrEmpty(item.Id)) 
                item.Id = Guid.NewGuid().ToString();
                var db = redis.GetDatabase();
                var json = JsonConvert.SerializeObject(item);
                var score = DateTime.UtcNow.Ticks;
                var result = db.SortedSetAdd(_optionsAccessor.Value.SetName, json, score);
            } catch (Exception ex) {
                return BadRequest(ex);
            }
            
            return Json("ok");
        }
    }
}
