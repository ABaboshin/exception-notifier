using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestListener.Config;
using RestListener.Models;
using StackExchange.Redis;

namespace RestListener.Controllers {
    [Route("api/[controller]")]
    public class NotifyController : Controller
    {
        private readonly IOptions<ListenerOptions> _optionsAccessor;

        public NotifyController(IOptions<ListenerOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Notify item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            try {
                var redis = ConnectionMultiplexer.Connect(_optionsAccessor.Value.RedisHost);
                var sub = redis.GetSubscriber();
                var json = JsonConvert.SerializeObject(item);
                sub.Publish(_optionsAccessor.Value.ChannelName, json);
            } catch (Exception ex) {
                return BadRequest(ex);
            }
            
            return Json("ok");
        }
    }
}