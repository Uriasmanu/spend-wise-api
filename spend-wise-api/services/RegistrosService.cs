using Microsoft.Extensions.Options;
using MongoDB.Driver;
using spend_wise_api.data;
using spend_wise_api.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace spend_wise_api.services
{
    public class RegistrosService
    {
        private readonly IMongoCollection<Registros> _registrosCollection;

        public RegistrosService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _registrosCollection = mongoDatabase.GetCollection<Registros>("Registros"); // Nome da coleção
        }

        public async Task AddRegistroAsync(Registros registro)
        {
            await _registrosCollection.InsertOneAsync(registro);
        }

        public async Task<List<Registros>> GetRegistrosAsync()
        {
            return await _registrosCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Registros> GetRegistroAsync(string identificador)
        {
            // Tenta encontrar o registro
            var registro = await _registrosCollection.Find(r => r.Identificador == identificador).FirstOrDefaultAsync();

            // Se o registro não for encontrado, lança uma exceção
            if (registro == null)
            {
                throw new KeyNotFoundException($"Registro com identificador '{identificador}' não encontrado.");
            }

            return registro;
        }

        public async Task UpdateRegistroAsync(string identificador, Registros updatedRegistro)
        {
            var result = await _registrosCollection.ReplaceOneAsync(r => r.Identificador == identificador, updatedRegistro);

            // Se nenhum registro foi atualizado, lança uma exceção
            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Registro com identificador '{identificador}' não encontrado para atualização.");
            }
        }

        public async Task DeleteRegistroAsync(string identificador)
        {
            var deleteResult = await _registrosCollection.DeleteOneAsync(r => r.Identificador == identificador);

            // Se nenhum registro foi deletado, lança uma exceção
            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Registro com identificador '{identificador}' não encontrado para deleção.");
            }
        }
    }
}
