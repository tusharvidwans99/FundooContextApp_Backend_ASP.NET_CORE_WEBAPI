﻿using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{

    public class NotesRL : INotesRL
    {

        private readonly FundooContext fundooContext;

        public NotesRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public NotesEntity AddNotes(NoteModel notesCreateModel, long userId)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();
                var result = fundooContext.UserTable.FirstOrDefault(e => e.UserId == userId);


                if (result != null)
                {
                    notesEntity.Title = notesCreateModel.Title;
                    notesEntity.Description = notesCreateModel.Description;
                    notesEntity.Reminder = notesCreateModel.Reminder;
                    notesEntity.Color = notesCreateModel.Color;
                    notesEntity.Image = notesCreateModel.Image;
                    notesEntity.Archive = notesCreateModel.Archive;
                    notesEntity.Pin = notesCreateModel.Pin;
                    notesEntity.Trash = notesCreateModel.Trash;
                    notesEntity.Created = notesCreateModel.Created;
                    notesEntity.Edited = notesCreateModel.Edited;
                    notesEntity.UserId = userId;
                    fundooContext.NotesTable.Add(notesEntity);
                    fundooContext.SaveChanges();

                    return notesEntity;
                }
                else
                {
                    return null;
                }
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
                var result = this.fundooContext.NotesTable.Where(x => x.UserId == userId);
                return result;
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
                var result = fundooContext.NotesTable.Where(note => note.UserId == userId && note.noteID == NoteId).FirstOrDefault();
                if (result != null)
                {
                    result.Title = noteModel.Title;
                    result.Description = noteModel.Description;
                    result.Reminder = noteModel.Reminder;
                    result.Edited = DateTime.Now;
                    result.Color = noteModel.Color;
                    result.Image = noteModel.Image;

                    this.fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
