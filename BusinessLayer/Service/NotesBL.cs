using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{

    public class NotesBL : INotesBL
    {
        private readonly INotesRL iNotesRL;

        public NotesBL(INotesRL iNotesRL)
        {
            this.iNotesRL = iNotesRL;
        }

        public NotesEntity AddNotes(NoteModel notesCreateModel, long userId)
        {
            try
            {
                return iNotesRL.AddNotes(notesCreateModel, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IEnumerable<NotesEntity> ReadNotes(long userId)
        {
            try
            {
                return iNotesRL.ReadNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public NotesEntity UpdateNote(NoteModel noteModel, long NoteId, long userId)
        {
            try
            {
                return iNotesRL.UpdateNote(noteModel, NoteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public bool DeleteNotes(long userId, long noteId)
        {
            try
            {
                return iNotesRL.DeleteNotes(userId, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool PinToTop(long NoteID, long userId)
        {
            try
            {
                return iNotesRL.PinToTop(NoteID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool Archive(long NoteID, long userId)
        {
            try
            {
                return iNotesRL.Archive(NoteID, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Trash(long NoteID, long userId)
        {
            try
            {
                return iNotesRL.Trash(NoteID, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity Color(long NoteID, string color)
        {
            try
            {

                return iNotesRL.Color(NoteID, color);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string Image(IFormFile image, long noteID, long userID)
        {
            try
            {
                return iNotesRL.Image(image, noteID, userID);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }

}
