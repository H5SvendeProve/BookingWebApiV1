using BookingWebApiV1.Authentication.ApiKey;
using BookingWebApiV1.Models.DatabaseResultDTOs;
using BookingWebApiV1.Services.WashingMachineService;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebApiV1.Controllers
{
    [ServiceFilter(typeof(ApiKeyAuthorizationFilter))]
    [ApiController]
    [Route("api/washingMachine")]
    public class WashingMachineController : ControllerBase
    {
        private IWashingMachineService WashingMachineService { get; }

        public WashingMachineController(IWashingMachineService washingMachineService)
        {
            WashingMachineService = washingMachineService;
        }


        
        [HttpGet("getBookingProgramDummyData")]
        public Task<IActionResult> GetBookingProgramDummyData(string rfid)
        {
            if (string.IsNullOrEmpty(rfid))
            {
                return Task.FromResult<IActionResult>(BadRequest("rfid is missing"));
            }

            // validate rfid
            bool validRfid = true;

            if (!validRfid)
            {
                return Task.FromResult<IActionResult>(BadRequest("not a valid rfid"));
            }

            return Task.FromResult<IActionResult>(Ok(new
            {
                ProgramName = "Koge", ProgramRunTimeMinutes = 180, MachineManufacturer = "Samsung",
                ModelName = "Vaskemaskine"
            }));
        }


        [HttpGet("getBookingProgram")]
        public async Task<IActionResult> GetBookingProgram(string rfid)
        {
            DateTime scannedTime = DateTime.Now;

            if (string.IsNullOrEmpty(rfid) || string.IsNullOrWhiteSpace(rfid))

            {
                return BadRequest("rfid is missing");
            }

            // validate rfid
            bool validRfid = await WashingMachineService.RfidCardExists(rfid);

            if (!validRfid)
            {
                return BadRequest("not a valid rfid");
            }

            var booking = await WashingMachineService.GetBookingConnectedToRfid(rfid, scannedTime);

            if (booking.BookingId < 1)
            {
                return BadRequest("error on inserting booking");
            }

            var programData = await WashingMachineService.GetBookingProgram(booking);

            if (programData.Equals(default(ProgramResultDTO)))
            {
                return NotFound();
            }
            
            return Ok(programData);
        }
    }
}