namespace TaskManagerCleanArchitecture.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task<IEnumerable<TaskItem>> GetFilteredAsync(Guid? userId, TaskItemStatus? status, bool orderDesc);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId);
    Task UpdateAsync(TaskItem task);
}