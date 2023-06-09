﻿using MasterUniversityNonRelational.Api.Models;
using System.Diagnostics;

namespace MasterUniversityNonRelational.Api.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(Guid id);
        Task<Student> Save(Student studentData);
        Task<Student> Update(String Id, Student studentData);
        Task<bool> Delete(Guid id);
        Task <string> TestCase(Student studentData, int testCases);
        Task<List<Student>> TestStudentInsert(int testCases, List<UniversityData> universities);
        Task<Stopwatch> TestStudentUpdate(int testCase, List<Student> oldStudentData);
        Task<List<Student>> TestStudentGet(int testCase);
        Task<bool> TestStudentDelete(int testCase, List<Student> studentData);
    }
}
