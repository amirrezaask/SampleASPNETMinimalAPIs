using Microsoft.EntityFrameworkCore;
using Nexus.Shared.Models;

namespace Nexus.Backend.Handlers;

public static class SavedPasswordHandlers
{
    public static async Task<SavedPassword?> GetNote(string id, NexusDbContext dbContext) => await dbContext.SavedPasswords.FindAsync(id);
    public static async Task<List<SavedPassword?>> GetNotes(NexusDbContext dbContext) => await dbContext.SavedPasswords.ToListAsync();

    public static async Task<string> AddNote(NexusDbContext dbContext, SavedPassword savedPassword)
    {
        savedPassword.Id = Guid.NewGuid().ToString();
        await dbContext.SavedPasswords.AddAsync(savedPassword);
        await dbContext.SaveChangesAsync();
        return savedPassword.Id;
    }

    public static async Task DeleteNote(NexusDbContext dbContext, string id)
    {
        dbContext.Remove(new SavedPassword { Id = id });
        await dbContext.SaveChangesAsync();
    }
}
