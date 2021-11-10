namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class WebApplicationExtensions
{
    public static void MapAuth(this WebApplication app, string prefix)
    {
        app.MapPost($"{prefix}/auth/register", AuthHandler.Register);
        app.MapPost($"{prefix}/auth/login", AuthHandler.Login);
    }
    public static void MapNotes(this WebApplication app, string prefix)
    {
        app.MapGet($"{prefix}/notes", NotesHandlers.GetNotes).WithTags("Notes");
        app.MapPost($"{prefix}/notes", NotesHandlers.AddNote).WithTags("Notes");
        app.MapGet($"{prefix}/notes/" + "{id:guid}", NotesHandlers.GetNote).WithTags("Notes");
        app.MapDelete($"{prefix}/notes/" + "{id:guid}", NotesHandlers.DeleteNote).WithTags("Notes");
    }
    public static void MapSavedPassword(this WebApplication app, string prefix)
    {
        var groupName = "Saved Passwords";
        app.MapGet($"{prefix}/saved_password", NotesHandlers.GetNotes).WithTags("Saved Passwords");
        app.MapPost($"{prefix}/saved_password", NotesHandlers.AddNote).WithTags("Saved Passwords");
        app.MapGet($"{prefix}/saved_password/" + "{id:guid}", NotesHandlers.GetNote).WithTags("Saved Passwords");
        app.MapDelete($"{prefix}/saved_password/" + "{id:guid}", NotesHandlers.DeleteNote).WithTags("Saved Passwords");
    }

}