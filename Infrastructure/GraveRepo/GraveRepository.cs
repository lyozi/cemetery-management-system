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

        public void DeleteGrave(long graveID)
        {
            var grave = context.GraveItems
                              .Include(g => g.DeceasedList)
                              .Include(g => g.GraveUIPolygon)
                              .SingleOrDefault(g => g.Id == graveID);

            if (grave == null)
            {
                throw new Exception("The specified grave was not found.");
            }

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

        public Grave GetGraveByTableRowParcel(char table, short row, short parcel)
        {
            return context.GraveItems.SingleOrDefault(g => g.Table == table && g.Row == row && g.Parcel == parcel);
        }

        public Grave GetGraveFromPolygonId(long id)
        {
            return context.GraveItems
                          .Include(g => g.DeceasedList)
                          .FirstOrDefault(g => g.GraveUIPolygon != null && g.GraveUIPolygon.Id == id);
        }
    }
}