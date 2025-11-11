using Proiect_Netficks.Data.Repositories;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services
{
    public class SerialService : ISerialService
    {
        private readonly ISerialRepository _serialRepository;

        public SerialService(ISerialRepository serialRepository)
        {
            _serialRepository = serialRepository;
        }

        public async Task<IEnumerable<Serial>> GetAllSerialsAsync()
        {
            return await _serialRepository.GetAllSerialsAsync();
        }

        public async Task<Serial?> GetSerialByIdAsync(int id)
        {
            return await _serialRepository.GetSerialByIdAsync(id);
        }

        public async Task<IEnumerable<Serial>> SearchSerialsAsync(string? title, int? year, int? genreId)
        {
            return await _serialRepository.SearchSerialsAsync(title, year, genreId);
        }
    }
}
