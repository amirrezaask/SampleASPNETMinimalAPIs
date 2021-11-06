using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Web.Handlers;

public static class NotesHandlers
{
    public static async Task<Note?> GetNote(string id, NexusDbContext dbContext) => await dbContext.Notes.FindAsync(id);
    public static async Task<List<Note?>> GetNotes(NexusDbContext dbContext) => await dbContext.Notes.ToListAsync();

    public static async Task<string> AddNote(NexusDbContext dbContext, Note note)
    {
        note.Id = Guid.NewGuid().ToString();
        await dbContext.Notes.AddAsync(note);
        await dbContext.SaveChangesAsync();
        return note.Id;
    }

    public static async Task DeleteNote(NexusDbContext dbContext, string id)
    {
        dbContext.Remove(new Note {Id = id});
        await dbContext.SaveChangesAsync();
    }

}