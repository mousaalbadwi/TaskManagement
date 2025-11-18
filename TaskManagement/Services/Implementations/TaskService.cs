using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs.Tasks;
using TaskManagement.Api.Entities;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskDto>> GetByCriteriaAsync(long userId, TaskFilterDto filter)
        {
            var query = _context.Tasks
                .Include(t => t.Status)
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(t => t.Title.Contains(filter.Title));

            if (filter.StatusId.HasValue)
                query = query.Where(t => t.StatusId == filter.StatusId);

            return await query
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    FromDate = t.FromDate,
                    ToDate = t.ToDate,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name
                })
                .ToListAsync();
        }

        public async Task<TaskDto?> GetByIdAsync(long id, long userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Status)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                FromDate = task.FromDate,
                ToDate = task.ToDate,
                StatusId = task.StatusId,
                StatusName = task.Status.Name
            };
        }

        public async Task AddAsync(long userId, TaskCreateDto dto)
        {
            if (dto.FromDate > dto.ToDate)
                throw new ApplicationException("FromDate cannot be greater than ToDate");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                StatusId = dto.StatusId,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(long userId, TaskUpdateDto dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (task == null || task.UserId != userId)
                throw new ApplicationException("Task not found or you do not own this task.");

            if (dto.FromDate > dto.ToDate)
                throw new ApplicationException("FromDate cannot be greater than ToDate");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.FromDate = dto.FromDate;
            task.ToDate = dto.ToDate;
            task.StatusId = dto.StatusId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id, long userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task == null || task.UserId != userId)
                throw new ApplicationException("Task not found or you do not own this task.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
