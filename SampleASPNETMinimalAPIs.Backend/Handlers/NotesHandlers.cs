using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    public static async Task<IResult> DeleteNote(SampleASPNETMinimalAPIsDbContext dbContext, string id)
    {
        var note = await dbContext.Notes.Where(n => n.Id == id).FirstOrDefaultAsync();
        if (note is null)
        {
            return Results.NotFound();
        }
        dbContext.Notes.Remove(note);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }

}