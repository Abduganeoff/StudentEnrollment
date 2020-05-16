using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial10.DTOs.Request;
using Tutorial10.Entities;

namespace Tutorial10.Services
{
    public interface IDbService
    {
        Task<IEnumerable<Student>> GetStudents();
        void AddStudent(Student entity);
        void DeleteStudent(Student entity);
        void UpdateStudent(StudentRequest request);
        Task<bool> SaveAll();
        Task<Enrollment> EnrollStudentWithExistingStudy(StudentEnrollmentRequest request);
        Task<Enrollment> PromoteStudent(PromoteStudentRequest request);

    }
}
