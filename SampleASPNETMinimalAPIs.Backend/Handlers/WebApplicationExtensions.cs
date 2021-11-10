namespace SampleASPNETMinimalAPIs.Backend.Handlers;

public static class WebApplicationExtensions
{
    public static void MapAuth(this WebApplication app, string prefix)
    {
        app.MapPost($"{prefix}/auth/register", AuthHandler.Register);
        app.MapPost($"{prefix}/auth/login", AuthHandler.Login);
        app.MapGet($"{prefix}/auth/validate/", AuthHandler.ValidateToken);
    }
    public static void MapNotes(this WebApplication app, string prefix)
    {
        app.MapGet($"{prefix}/notes",  NotesHandlers.GetNotes).WithTags("Notes").RequireAuthorization();
        app.MapPost($"{prefix}/notes", NotesHandlers.AddNote).WithTags("Notes");
        app.MapGet($"{prefix}/notes/" + "{id:guid}", NotesHandlers.GetNote).WithTags("Notes");
        app.MapDelete($"{prefix}/notes/" + "{id:guid}", NotesHandlers.DeleteNote).WithTags("Notes");
    }

    public static WebApplication MapAPIs(this WebApplication app)
    {
                
        app.MapNotes("/api/v1");
        app.MapAuth("/api/v1");
        return app;
    }

}