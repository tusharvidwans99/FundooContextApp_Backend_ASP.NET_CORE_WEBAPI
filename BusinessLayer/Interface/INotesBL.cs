using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {

        public NotesEntity AddNotes(NoteModel notesCreateModel, long userId);

        public IEnumerable<NotesEntity> ReadNotes(long userId);

        public NotesEntity UpdateNote(NoteModel noteModel, long NoteId, long userId);

    }
}
