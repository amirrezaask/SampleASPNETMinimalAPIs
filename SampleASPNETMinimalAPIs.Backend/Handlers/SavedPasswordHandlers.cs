using Microsoft.EntityFrameworkCore;
using SampleASPNETMinimalAPIs.Shared.Models;

namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class SavedPasswordHandlers
{
    public static async Task<SavedPassword?> GetNote(string id, SampleASPNETMinimalAPIsDbContext dbContext) => await dbContext.SavedPasswords.FindAsync(id);
    public static async Task<List<SavedPassword?>> GetNotes(SampleASPNETMinimalAPIsDbContext dbContext) => await dbContext.SavedPasswords.ToListAsync();

    public static async Task<string> AddNote(SampleASPNETMinimalAPIsDbContext dbContext, SavedPassword savedPassword)
    {
        savedPassword.Id = Guid.NewGuid().ToString();
        await dbContext.SavedPasswords.AddAsync(savedPassword);
        await dbContext.SaveChangesAsync();
        return savedPassword.Id;
    }

    public static async Task DeleteNote(SampleASPNETMinimalAPIsDbContext dbContext, string id)
    {
        dbContext.Remove(new SavedPassword { Id = id });
        await dbContext.SaveChangesAsync();
    }
}
