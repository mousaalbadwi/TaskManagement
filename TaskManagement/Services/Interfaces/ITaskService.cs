using TaskManagement.Api.DTOs.Tasks;

namespace TaskManagement.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetByCriteriaAsync(long userId, TaskFilterDto filter);
        Task<TaskDto?> GetByIdAsync(long id, long userId);
        Task AddAsync(long userId, TaskCreateDto dto);
        Task UpdateAsync(long userId, TaskUpdateDto dto);
        Task DeleteAsync(long id, long userId);
    }
}
