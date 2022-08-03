﻿using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace RepositoryLayer.Service
{

    public class NotesRL : INotesRL
    {

        private readonly FundooContext fundooContext;
        private readonly IConfiguration cloudinaryEntity;

        public NotesRL(FundooContext fundooContext, IConfiguration cloudinaryEntity)
        {
            this.fundooContext = fundooContext;
            this.cloudinaryEntity = cloudinaryEntity;
        }
        public NotesEntity AddNotes(NoteModel notesCreateModel, long userId)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();
                var result = fundooContext.UserTable.FirstOrDefault(x => x.UserId == userId);


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
                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.noteID == NoteId).FirstOrDefault();
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


        public bool DeleteNotes(long userId, long noteId)
        {
            try
            {

                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.noteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.NotesTable.Remove(result);
                    this.fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
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
                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.noteID == NoteID).FirstOrDefault();

                if (result.Pin == true)
                {
                    result.Pin = false;
                    fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.Pin = true;
                    fundooContext.SaveChanges();
                    return true;
                }
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
                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.noteID == NoteID).FirstOrDefault();

                if (result.Archive == false)
                {
                    result.Archive = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Archive = false;
                    fundooContext.SaveChanges();
                    return false;
                }
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
                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.noteID == NoteID).FirstOrDefault();

                if (result.Trash == false)
                {
                    result.Trash = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Trash = false;
                    fundooContext.SaveChanges();
                    return false;
                }
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

                var result = fundooContext.NotesTable.First(x => x.noteID == NoteID);

                if (result != null)
                {
                    result.Color = color;
                    fundooContext.SaveChanges();
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



        public string Image(IFormFile image, long noteID, long userID)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserId == userID && x.noteID == noteID).FirstOrDefault();
                if (result != null)
                {
                    Account cldaccount = new Account(
                        cloudinaryEntity["CloudinarySettings:CloudName"],
                        cloudinaryEntity["CloudinarySettings:ApiKey"],
                        cloudinaryEntity["CloudinarySettings:ApiSecret"]
                        );
                    Cloudinary cloudinary = new Cloudinary(cldaccount);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = imagePath;
                    fundooContext.SaveChanges();
                    return "Image uploaded successfully";
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
