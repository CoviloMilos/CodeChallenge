using System.Collections.Generic;
using System.Threading.Tasks;
using Task.Api.Models;

namespace Task.Api.Interfaces
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task AddTask(TaskModel task);
        void Delete<T>(T entity) where T: class;
        Task<IEnumerable<TaskModel>> GetTasks(bool isProduction);
        Task<TaskModel> GetTaskById(int taskId);
        Task<TaskModel> GetTaskByGuid(string taskGuid);
        Task<Case> GetSpecificCase(string taskGuid, int caseNum);
        Task<bool> SaveAll();
    }
}