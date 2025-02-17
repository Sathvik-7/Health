using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles="Admin,Health Professional")]
    public class AdminHealthController : ControllerBase
    {
        private readonly IPatientInformation _patientInformation;

        public AdminHealthController(IPatientInformation patientInformation) 
        {
            _patientInformation = patientInformation;
        }

        [HttpGet("getAllPatientsInfo")]
        public async Task<ActionResult<IEnumerable<PatientModel>>> getAllPatientsInfo(
            [FromQuery]string? filterOn, [FromQuery]string? filterQuery,
            [FromQuery]string? sortBy, [FromQuery]bool? isAsc,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            var patients = await _patientInformation.getPatientDetails(filterOn,filterQuery,sortBy,isAsc??true);
            
            var skip = (pageNumber - 1 ) * pageSize;

            return Ok(patients.Skip(skip).Take(pageSize));
        }

        [HttpPost("getPatientInfoByID")]
        public async Task<ActionResult<PatientModel>> getPatientInfoByID([FromBody]PatienDetailsModel patienDetails)
        {
            var patients = await _patientInformation.getPatientDetailsById(patienDetails);

            if (patients == null)
                return BadRequest(new { Message = "Id/Name doesnt exist" });

            return Ok(patients);
        }
    }
}
