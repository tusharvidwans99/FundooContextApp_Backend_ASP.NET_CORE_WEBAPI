using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {

        public NotesEntity AddNotes(NoteCreateModel notesCreateModel, long userId);

        public IEnumerable<NotesEntity> ReadNotes(long userId);
    }
}
