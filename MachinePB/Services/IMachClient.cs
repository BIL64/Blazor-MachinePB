using ServerAPI.Entities;

namespace MachinePB.Services
{
    public interface IMachClient
    {
        Task<IEnumerable<Machine>?> GetAsync();
        Task<Machine?> GetAsync(string id);
        Task<Machine?> PostAsync(Machine machine);
        Task<bool> PutAsync(string id, Machine machine);
        Task<bool> RemoveAsync(string id);
    }
}