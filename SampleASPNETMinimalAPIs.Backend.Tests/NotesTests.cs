using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SampleASPNETMinimalAPIs.Backend.Handlers;
using SampleASPNETMinimalAPIs.Shared.Models;
using Xunit;

namespace SampleASPNETMinimalAPIs.Backend.Tests;

public class NotesTests : BaseHandlerTest
{
    [Fact]
    public void GetAllNotesTest()
    {
        // Arrange
        _dbContext.Notes.Add(new Note
        {
            Id = Guid.NewGuid().ToString(),
            Title = "test 1",
            Content = "test content 1"
        });
        _dbContext.SaveChanges();

        // Act
        var notes = NotesHandlers.GetNotes(_dbContext).Result;

        // Assert
        Assert.Single(notes);
    }

    [Fact]
    public void AddNoteTest()
    {
        // Act
        var newID = NotesHandlers.AddNote(_dbContext, new Note
        {
            Title = "new title",
            Content = "new content"
        }).Result;
        var note = _dbContext.Notes.Find(newID);
        // Assert
        Assert.Equal(newID, note.Id);
    }

    [Fact]
    public void GetNote()
    {
        var id = Guid.NewGuid().ToString();
        _dbContext.Add(new Note()
        {
            Id = id,
            Title = "title",
            Content = "content"
        });
        _dbContext.SaveChanges();
        var note = NotesHandlers.GetNote(id, _dbContext).Result;
        Assert.Equal(id, note.Id);
    }
}