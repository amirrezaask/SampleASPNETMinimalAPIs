using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleASPNETMinimalAPIs.Shared.Models;

namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class NotesHandlers
{
    public static string Sample() => "Hello World";
    public static async Task<Note?> GetNote(string id, SampleASPNETMinimalAPIsDbContext dbContext) => await dbContext.Notes.FindAsync(id);
    public static async Task<List<Note?>> GetNotes(SampleASPNETMinimalAPIsDbContext dbContext) => await dbContext.Notes.ToListAsync();

    public static async Task<string> AddNote(SampleASPNETMinimalAPIsDbContext dbContext, Note note)
    {
        note.Id = Guid.NewGuid().ToString();
        await dbContext.Notes.AddAsync(note);
        await dbContext.SaveChangesAsync();
        return note.Id;
    }

    public static async Task DeleteNote(SampleASPNETMinimalAPIsDbContext dbContext, string id)
    {
        dbContext.Remove(new Note { Id = id });
        await dbContext.SaveChangesAsync();
    }

}