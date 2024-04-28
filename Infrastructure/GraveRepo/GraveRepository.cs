using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.Models;
using Domain.RepositoryInterfaces;

namespace Infrastructure.GraveRepo
{
    public class GraveRepository : IGraveRepository, IDisposable
    {
        private DatabaseContext context;

        public GraveRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Grave> GetGraves()
        {
            return context.GraveItems.ToList();
        }

        public Grave GetGraveByID(long id)
        {
            return context.GraveItems.Find(id);
        }

        public void InsertGrave(Grave deceased)
        {
            context.GraveItems.Add(deceased);
        }

        public void DeleteGrave(long deceasedID)
        {
            var grave = context.GraveItems
                              .Include(g => g.DeceasedList)
                              .Include(g => g.GraveUIPolygon)
                              .SingleOrDefault(g => g.Id == deceasedID);

            if (grave == null)
            {
                throw new Exception("A megadott sír nem található.");
            }

            grave.DeceasedList = null;
            grave.GraveUIPolygon = null;

            Save();

            context.GraveItems.Remove(grave);
            Save();
        }

        public void UpdateGrave(Grave deceased)
        {
            context.Entry(deceased).State = EntityState.Modified;
        }

        public bool GraveExists(long id)
        {
            return context.GraveItems.Any(d => d.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Grave GetGraveFromPolygonId(long id)
        {
            return context.GraveItems
                          .Include(g => g.DeceasedList)
                          .FirstOrDefault(g => g.GraveUIPolygon != null && g.GraveUIPolygon.Id == id);
        }
    }
}