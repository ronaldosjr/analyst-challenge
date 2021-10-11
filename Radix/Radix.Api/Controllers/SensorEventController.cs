using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Radix.Api.HubConfig;
using Radix.Api.ViewModel;
using Radix.Domain.Entities;
using Radix.Domain.Exceptions;
using Radix.Domain.Interfaces;

namespace Radix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorEventController : ControllerBase
    {
        private readonly ISensorEventService _sensorEventService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SensorEventHub> _hub;

        public SensorEventController(
            ISensorEventService sensorEventService, 
            IMapper mapper,
            IHubContext<SensorEventHub> hub)
        {
            _sensorEventService = sensorEventService;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SensorEventViewModel sensorEventViewModel)
        {
            try
            {
                var sensorEvent = _mapper.Map<SensorEvent>(sensorEventViewModel);
                await _sensorEventService.InsertAsync(sensorEvent);
                await _hub.Clients.All.SendAsync("sensoreventdata", sensorEvent);

                return Ok(sensorEvent);
            }
            catch (RadixException r)
            {
                return BadRequest(r.Message);
            }
            
        }

        [HttpGet("aggregate")]
        public async Task<IList<SensorEventAggregate>> GetAggergatesAsync()
        {
            return await Task.Run(() => _sensorEventService.GetAggregate());
        }

        [HttpGet()]
        public async Task<IList<SensorEvent>> GetLatestAsync()
        {
            return await Task.Run(() => _sensorEventService.GetLatests());
        }

    }
}
