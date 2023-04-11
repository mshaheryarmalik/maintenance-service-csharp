using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryDevicesController : ControllerBase
    {
        private readonly IFactoryDeviceService _factoryDeviceService;

        public FactoryDevicesController(IFactoryDeviceService factoryDeviceService)
        {
            _factoryDeviceService = factoryDeviceService;
        }

        // GET: api/FactoryDevices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var devices = await _factoryDeviceService.GetAll();
                var deviceDtos = devices.Select(fd =>
                    new FactoryDeviceDto
                    {
                        Id = fd.Id,
                        Name = fd.Name,
                        Year = fd.Year,
                        Type = fd.Type
                    }).ToList();
                return Ok(deviceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/FactoryDevices/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var fd = await _factoryDeviceService.Get(id);
                if (fd == null)
                {
                    return NotFound();
                }

                var deviceDto = new FactoryDeviceDto
                {
                    Id = fd.Id,
                    Name = fd.Name,
                    Year = fd.Year,
                    Type = fd.Type
                };
                return Ok(deviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST: api/FactoryDevices
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FactoryDeviceDto deviceDto)
        {
            try
            {
                var device = new FactoryDevice
                {
                    Name = deviceDto.Name,
                    Year = deviceDto.Year,
                    Type = deviceDto.Type
                };
                await _factoryDeviceService.Create(device);

                return CreatedAtAction("Get", new { id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/FactoryDevices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FactoryDeviceDto deviceDto)
        {
            try
            {
                var device = await _factoryDeviceService.Get(id);
                if (device == null)
                {
                    return NotFound();
                }

                device.Name = deviceDto.Name;
                device.Year = deviceDto.Year;
                device.Type = deviceDto.Type;

                await _factoryDeviceService.Update(device);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/FactoryDevices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var device = await _factoryDeviceService.Get(id);
                if (device == null)
                {
                    return NotFound();
                }

                await _factoryDeviceService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/factorydevices/search?type=abc
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? type)
        {
            try
            {
                var devices = await _factoryDeviceService.FilterDevicesByType(type);
                if (devices == null || devices.Count() == 0)
                {
                    return NotFound();
                }

                var dtoList = devices.Select(fd => new FactoryDeviceDto
                {
                    Id = fd.Id,
                    Name = fd.Name,
                    Year = fd.Year,
                    Type = fd.Type
                });

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
