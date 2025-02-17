using HealthCareSystem.Context;
using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Serilog;

namespace HealthCareSystem.Repository.Implementation
{
    public class PatientInformation : IPatientInformation
    {
        private readonly HealthCareContext _dbContext;

        public PatientInformation(HealthCareContext _dbContext) 
        {
            this._dbContext = _dbContext;
        }

        async Task<List<PatientModel>> IPatientInformation.getPatientDetails(string? filterOn = null, string? filterQuery = null,
                                                    string? sortBy = null, bool isAsc = true)
        {
            IQueryable<Patient> patientsQuery = _dbContext.Patients;

            
            try 
            {
                // **Apply Filtering**
                if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
                {
                    var propertyInfo = typeof(Patient).GetProperty(filterOn,
                                      System.Reflection.BindingFlags.IgnoreCase |
                                      System.Reflection.BindingFlags.Public |
                                      System.Reflection.BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        var param = Expression.Parameter(typeof(Patient), "p");
                        var property = Expression.Property(param, propertyInfo);
                        var filterValue = Expression.Constant(Convert.ChangeType(filterQuery, propertyInfo.PropertyType));
                        var equalsExpression = Expression.Equal(property, filterValue);
                        var lambda = Expression.Lambda<Func<Patient, bool>>(equalsExpression, param);

                        patientsQuery = patientsQuery.Where(lambda);
                    }
                }

                // Apply Sorting if sortBy is provided
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var propertyInfo = typeof(Patient).GetProperty(sortBy,
                                      System.Reflection.BindingFlags.IgnoreCase |
                                      System.Reflection.BindingFlags.Public |
                                      System.Reflection.BindingFlags.Instance);

                    if (propertyInfo != null)
                    {
                        var param = Expression.Parameter(typeof(Patient), "p");
                        var property = Expression.Property(param, propertyInfo);
                        var lambda = Expression.Lambda(property, param);

                        var methodName = isAsc ? "OrderBy" : "OrderByDescending";
                        var orderByMethod = typeof(Queryable).GetMethods()
                            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(Patient), propertyInfo.PropertyType);

                        patientsQuery = (IQueryable<Patient>)orderByMethod.Invoke(null, new object[] { patientsQuery, lambda });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failure : {@RequestName} , {@Error} , {@DateTimeUTC}",
                            "getPatientDetails", ex.Message, DateTime.Today);
            }
            
            return await patientsQuery.Select(p => new PatientModel()
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                ContactNumber = p.ContactNumber,
                Email = p.Email,
                Address = p.Address,
                Recommendations = p.Recommendations,
            }).ToListAsync();
        }

        async Task<PatientModel> IPatientInformation.getPatientDetailsById(PatienDetailsModel patienDetails)
        {
            var result = await _dbContext.Patients
                                        .Where(p => (p.PatientId == patienDetails.PatientId
                                                    && p.FirstName == patienDetails.FirstName))
                                        .Select(p => new PatientModel()
                                        {
                                            //PatientId = p.PatientId,
                                            FirstName = p.FirstName,
                                            LastName = p.LastName,
                                            DateOfBirth = p.DateOfBirth,
                                            Gender = p.Gender,
                                            ContactNumber = p.ContactNumber,
                                            Email = p.Email,
                                            Address = p.Address,
                                            Recommendations = p.Recommendations,
                                        })
                                        .FirstOrDefaultAsync();


            return result;
        }
    }
}
