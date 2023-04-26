using AzHttpAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzHttpAPI.Services
{
    public class DepartmentService : IServices<Department, int>
    {
        // CompanyContext context = new CompanyContext();
        CompanyContext context;
        ResponseObject<Department> resonse = new ResponseObject<Department>();

        public DepartmentService(CompanyContext context)
        {
            this.context = context;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.CreateAsync(Department entity)
        {
            var result = await context.Departments.AddAsync(entity);
            await context.SaveChangesAsync(); // COmmit Transactions
            resonse.Record = result.Entity;
            resonse.StatucCode = 201;
            resonse.StatusMessage = "Department is ceated successfuly";
            return resonse;

        }

        async Task<ResponseObject<Department>> IServices<Department, int>.DeleteAsync(int id)
        {
           var record = await context.Departments.FindAsync(id);
            if (record == null)
            {
                resonse.StatucCode = 404;
                resonse.StatusMessage = "Department is not found";
                return resonse;
            }
            context.Departments.Remove(record);
            await context.SaveChangesAsync();
            resonse.StatucCode = 202;
            resonse.StatusMessage = "Department is deleted successfully";
            return resonse;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.GetAsync()
        {
            resonse.Records = await context.Departments.ToListAsync();
            resonse.StatucCode = 200;
            resonse.StatusMessage = "Departments are read successfully";
            return resonse;
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.GetAsync(int id)
        {
            var record = await context.Departments.FindAsync(id);
            if (record == null)
            {
                resonse.StatucCode = 404;
                resonse.StatusMessage = "Department is not found";
                return resonse;
            }
            resonse.Record = record;
            resonse.StatucCode = 200;
            resonse.StatusMessage = "Department is  found";
            return resonse; 
        }

        async Task<ResponseObject<Department>> IServices<Department, int>.UpdateAsync(int id, Department entity)
        {
           context.Entry<Department>(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();   
            resonse.StatucCode = 200;
            resonse.Record = entity;
            resonse.StatusMessage = "Department updated successfuy";
            return resonse;

        }
    }
}
