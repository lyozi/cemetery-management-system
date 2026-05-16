using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class GravesService : IGravesService
    {
        private readonly IGraveRepository _graveRepository;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<GravesService> _log;

        public GravesService(
            IGraveRepository graveRepository,
            IImageStorage imageStorage,
            ILogger<GravesService> log)
        {
            _graveRepository = graveRepository;
            _imageStorage = imageStorage;
            _log = log;
        }

        public IEnumerable<Grave> GetGraves()
        {
            return _graveRepository.GetGraves();
        }

        public Grave GetGraveByID(long id)
        {
            return _graveRepository.GetGraveByID(id);
        }

        public void InsertGrave(Grave grave)
        {
            _graveRepository.InsertGrave(grave);
            _graveRepository.Save();
        }

        public void UpdateGrave(Grave grave)
        {
            _graveRepository.UpdateGrave(grave);
            _graveRepository.Save();
        }

        public async Task DeleteGraveAsync(long id, CancellationToken ct = default)
        {
            var grave = _graveRepository.GetGraveWithDeceasedListByID(id);
            if (grave is null) return;

            var urls = new List<string?> { grave.ImageUrl };
            if (grave.DeceasedList is not null)
            {
                urls.AddRange(grave.DeceasedList.Select(d => d.ImageUrl));
            }

            _graveRepository.DeleteGrave(id);

            foreach (var url in urls.Where(u => !string.IsNullOrEmpty(u)))
            {
                try
                {
                    await _imageStorage.DeleteAsync(url!, ct);
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Image cleanup failed for {Url}", url);
                }
            }
        }

        public async Task DeleteAllGravesAsync(CancellationToken ct = default)
        {
            var graves = _graveRepository.GetGravesWithDeceasedList().ToList();

            var urls = graves
                .SelectMany(g => new[] { g.ImageUrl }
                    .Concat(g.DeceasedList?.Select(d => d.ImageUrl) ?? Enumerable.Empty<string?>()))
                .Where(u => !string.IsNullOrEmpty(u))
                .ToList();

            foreach (var g in graves)
            {
                _graveRepository.DeleteGrave(g.Id);
            }

            foreach (var url in urls)
            {
                try
                {
                    await _imageStorage.DeleteAsync(url!, ct);
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Image cleanup failed for {Url}", url);
                }
            }
        }

        public bool GraveExists(long id)
        {
            return _graveRepository.GraveExists(id);
        }

        public Grave GetOrCreateGrave(char table, short row, short parcel)
        {
            var grave = _graveRepository.GetGraveByTableRowParcel(table, row, parcel);
            if (grave == null)
            {
                grave = new Grave
                {
                    Table = table,
                    Row = row,
                    Parcel = parcel
                };
                _graveRepository.InsertGrave(grave);
                _graveRepository.Save();
            }
            return grave;
        }

        public Grave GetGraveFromPolygonId(long id)
        {
            return _graveRepository.GetGraveFromPolygonId(id);
        }

        public async Task<string> SetGraveImageAsync(
            long graveId,
            Stream content,
            string fileName,
            string contentType,
            CancellationToken ct = default)
        {
            var grave = _graveRepository.GetGraveByID(graveId)
                        ?? throw new InvalidOperationException($"Grave {graveId} not found.");

            var url = await _imageStorage.UploadAsync(content, fileName, contentType, $"graves/{graveId}", ct);
            grave.ImageUrl = url;
            _graveRepository.UpdateGrave(grave);
            _graveRepository.Save();
            return url;
        }
    }
}
