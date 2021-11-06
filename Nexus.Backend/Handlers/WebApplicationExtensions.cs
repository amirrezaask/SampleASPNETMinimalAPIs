namespace Nexus.Web.Handlers;

public static class WebApplicationExtensions {
    public static void MapNotes(this WebApplication app, string prefix)
    {
        app.MapGet($"{prefix}/notes", NotesHandlers.GetNotes);
        app.MapPost($"{prefix}/notes", NotesHandlers.AddNote);
        app.MapGet($"{prefix}/notes/" + "{id:guid}", NotesHandlers.GetNote);
        app.MapDelete($"{prefix}/notes/" + "{id:guid}", NotesHandlers.DeleteNote);
    }
}