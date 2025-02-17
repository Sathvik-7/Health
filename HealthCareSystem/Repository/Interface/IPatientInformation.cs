using HealthCareSystem.Models.DTO;

namespace HealthCareSystem.Repository.Interface
{
    public interface IPatientInformation
    {
        Task<List<PatientModel>> getPatientDetails(string? filterOn=null,string? filterQuery=null,
                                                    string? sortBy=null,bool isAsc=true);

        Task<PatientModel> getPatientDetailsById(PatienDetailsModel patienDetails);
    }
}
