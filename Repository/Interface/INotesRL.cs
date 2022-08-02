using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {

        public NotesEntity AddNotes(NoteModel notesCreateModel, long userId);

        public IEnumerable<NotesEntity> ReadNotes(long userId);

        public NotesEntity UpdateNote(NoteModel noteModel, long NoteId, long userId);

        public bool DeleteNotes(long userId, long noteId);

        public bool PinToTop(long NoteID, long userId);
        public bool Archive(long NoteID, long userId);

        public bool Trash(long NoteID, long userId);
    }
}
