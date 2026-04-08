using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Travel_Agent.Auth.AuthModel
{
    public class Employee:IEmployee
    {
        private const string ORG = "TA";
        private readonly UserManager<ApplicationUser> _userManager;

     public Employee(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

public async Task<string> GenerateEmployeeId()
{
    var year = DateTime.Now.Year;

    var lastEmployeeId = _userManager.Users
        .Where(u => u.EmployeeID != null && u.EmployeeID.Contains(year.ToString()))
        .OrderByDescending(u => u.EmployeeID)
        .Select(u => u.EmployeeID)
        .FirstOrDefault();

    int nextNumber = 1;

    if (!string.IsNullOrEmpty(lastEmployeeId))
    {
        var lastNumber = lastEmployeeId.Split('-').Last();
        nextNumber = int.Parse(lastNumber) + 1;
    }
    

    return $"{ORG}/{year}/{nextNumber.ToString("D3")}";
}
    }

    public interface IEmployee
    {
        Task<string> GenerateEmployeeId();
    }
}