using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Task.Api.Interfaces;
using Task.Api.Models;

namespace Task.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private TaskDataContext _context;

        public TaskRepository(TaskDataContext context) 
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task AddTask(TaskModel task)
        {
            await _context.AddAsync(task);

            foreach (var item in task.Cases)
            {
                await _context.AddAsync(item);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                _context.Remove(entity);
            }
            catch (System.Exception)
            {
                throw new CustomException("task_delete_failed", $"Task delete failed.");  
            }
        }

        public async Task<Case> GetSpecificCase(string taskGuid, int caseNum)
        {
            try
            {
                return await (from cases in _context.Cases
                              where cases.TaskGuid == Guid.Parse(taskGuid) && cases.CaseNum == caseNum
                              select cases).SingleOrDefaultAsync();
            }
            catch (System.Exception)
            {
                throw new CustomException("case_get_specific_failed", $"Get specific case failed"); 
            }
        }

        public async Task<TaskModel> GetTaskByGuid(string taskGuid)
        {
            return await _context.Tasks.Include(t => t.Cases).FirstOrDefaultAsync(t => t.TaskGuid == Guid.Parse(taskGuid));
        }

        public async Task<TaskModel> GetTaskById(int taskId)
        {
            return await _context.Tasks.Include(t => t.Cases).FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<IEnumerable<TaskModel>> GetTasks(bool isProduction)
        {
           return await _context.Tasks
                            .Include(t => t.Cases)
                            .OrderBy(t => t.TaskNum)
                            .Where(t => t.IsProdcution == isProduction)
                            .ToListAsync();
        }

        public async Task<bool> SaveAll()
        {   
            var a = true;
            try
            {
              a= await _context.SaveChangesAsync() > 0;   
            }
            catch (System.Exception)
            {
                throw new CustomException("task_repository_saveall_failed", $"Task repository failed on SaveAll() method."); 
            }
            return a;
        }
    }
}