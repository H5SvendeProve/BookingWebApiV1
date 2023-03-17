using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Services.FrontendService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace BookingWebApiV1.Controllers
{
    [ApiController]
    [Route("api/frontend")]
    public class FrontendController : ControllerBase
    {
        private IFrontendService FrontendService { get; }

        public FrontendController(IFrontendService frontendService)
        {
            FrontendService = frontendService;
        }

        [HttpGet("validateToken")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", string.Empty);

            if (token.IsNullOrEmpty())
            {
                return BadRequest("missing jwt token");
            }

            bool isValid = await FrontendService.ValidateToken(token);

            if (!isValid)
            {
                return Unauthorized("token is invalid or expired");
            }

            return Ok(isValid);
        }


        [HttpPost("createNewMachine")]
        [Authorize]
        public async Task<IActionResult> CreateNewMachine([FromBody] CreateNewMachineRequest createNewMachineRequest)
        {
            var isInserted = await FrontendService.CreateNewMachine(createNewMachineRequest);

            if (!isInserted)
            {
                return BadRequest("error on inserting of the machine");
            }

            return Created("result", createNewMachineRequest);
        }

        //[Authorize]
        [HttpPost("createNewBooking")]
        public async Task<IActionResult> CreateNewBooking([FromBody] CreateNewBookingRequest newBookingRequest)
        {
            try
            {
                var newBooking = await FrontendService.CreateNewBooking(newBookingRequest);

                if (newBooking.BookingId < 1)
                {
                    return NotFound("booking is not presented in the database");
                }

                return Created("booking", newBooking);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("createNewRfidCard")]
        public async Task<IActionResult> CreateNewRfidCard([FromBody] CreateNewRfidCardRequest rfidCardRequest)
        {
            var rfidCreated = await FrontendService.CreateNewRfidCard(rfidCardRequest);

            if (!rfidCreated)
            {
                return NotFound("rfid not presented to database");
            }

            return Created("result", rfidCreated);
        }

        [Authorize]
        [HttpPost("createNewArduinoMaster")]
        public async Task<IActionResult> CreateNewArduinoMaster(CreateMasterArduinoRequest createMasterArduinoRequest)
        {
            var result = await FrontendService.CreateNewMasterArduino(createMasterArduinoRequest);

            if (result.MasterArduinoId.Length < 1)
            {
                return NotFound("masterArduino not created");
            }

            return Created("masterArduino", result);
        }

        [Authorize]
        [HttpPost("createNewArduinoMachine")]
        public async Task<IActionResult> CreateNewArduinoMachine(
            CreateArduinoMachineRequest createArduinoMachineRequest)
        {
            var result = await FrontendService.CreateNewArduinoMachine(createArduinoMachineRequest);

            if (result.MasterArduinoId.Length < 1)
            {
                return NotFound("arduino machine not found");
            }

            return Created("ArduinoMachine", result);
        }

        [Authorize]
        [HttpGet("getMachinesByArduinoMasterId")]
        public async Task<IActionResult> GetMachinesByArduinoMasterId([FromQuery] string arduinoMasterId)
        {
            var result = await FrontendService.GetMachinesByArduinoMasterId(arduinoMasterId);

            if (!result.Any())
            {
                return NotFound($"no machines connected to the arduinoMaterId {arduinoMasterId}");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("getAvailableBookingTimes")]
        public async Task<IActionResult> GetAvailableBookingTimes(string username)
        {
            var result = await FrontendService.GetAvailableBookingTimes(username);

            if (!result.Any())
            {
                return NotFound($"there's no available booking times for user {username}");
            }

            return Ok(result);
        }
    }
}