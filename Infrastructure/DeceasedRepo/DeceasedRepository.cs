using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.DeceasedRepo
{
    public class DeceasedRepository : IDeceasedRepository, IDisposable
    {
        private DatabaseContext context;

        public DeceasedRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Deceased> GetDeceaseds()
        {
            return context.DeceasedItems.ToList();
        }

        public Deceased GetDeceasedByID(long id)
        {
            return context.DeceasedItems.Find(id);
        }

        public async Task<Deceased> GetDeceasedWithMessagesByID(long id)
        {
            var deceased = await context.DeceasedItems.Include(d => d.MessageList).FirstOrDefaultAsync(d => d.Id == id);
            return deceased;
        }

        public bool InsertDeceased(Deceased deceased)
        {
            try
            {
                context.DeceasedItems.Add(deceased);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt az elhunyt hozzáadása során: {ex.Message}");
                return false;
            }
        }

        public void DeleteDeceased(long deceasedID)
        {
            Deceased deceased = context.DeceasedItems
                                    .Include(d => d.MessageList)
                                    .FirstOrDefault(d => d.Id == deceasedID);

            if (deceased != null)
            {
                if (deceased.MessageList != null)
                {
                    context.MessageItems.RemoveRange(deceased.MessageList);
                }
                context.DeceasedItems.Remove(deceased);
                Save();
            }
        }


        public void UpdateDeceased(Deceased deceased)
        {
            context.Entry(deceased).State = EntityState.Modified;
        }

        public bool DeceasedExists(long id)
        {
            return context.DeceasedItems.Any(d => d.Id == id);
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
    }
}