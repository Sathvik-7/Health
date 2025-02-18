using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

        [HttpPost("insertPatientDetails")]
        public async Task<IActionResult> insertPatientDetails([FromBody]PatientModel patienDetails)
        {
            int records = await _patientInformation.insertPatientInfo(patienDetails);

            if (records == 0)
                return BadRequest(new {Message = "Inserting details failed"});

            return Ok(new {Message = "Patient Details inserted successful"});
        }

        [HttpPost("getRecommendationByID")]
        public async Task<IActionResult> getRecommendationByID([FromBody] PatienDetailsModel patienDetails)
        {
            var patients = await _patientInformation.getPatientDetailsById(patienDetails);

            if(patients == null)
                return BadRequest(new { Message = "Id/Name doesnt exist" });
            
            var patientRec = new RecommendationModel()
            {
                FirstName = patients.FirstName,
                LastName = patients.LastName,
                DateOfBirth = patients.DateOfBirth,
                Gender = patients.Gender,  
                Recommendations = patients.Recommendations
            };

            return Ok(new {Patients = patientRec });
        }
    
    }
}
