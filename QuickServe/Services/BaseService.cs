using QuickServe.Entities;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public abstract class BaseService<T> : IBaseService<T>
        where T : IdentifiableEntity
    {
        private IJsonService _jsonService;
        private string _fileName;

        public BaseService(IJsonService jsonService, string fileName)
        {
            this._jsonService = jsonService;
            this._fileName = fileName;
        }

        public List<T> GetAll()
        {
            var entities = this._jsonService.GetFileContent<List<T>>(this._fileName);
            return entities ?? [];
        }

        public T? GetById(Guid uuid)
        {
            var entities = this._jsonService.GetFileContent<List<T>>(this._fileName);
            if (entities == null)
                return null;

            if (!entities.Any(entity => entity.Uuid == uuid))
                return null;

            return entities.FirstOrDefault(entity => entity.Uuid == uuid);
        }

        public bool Create(T entity)
        {
            var entities = this._jsonService.GetFileContent<List<T>>(this._fileName);
            if (entities == null)
                return false;

            entities.Add(entity);
            return this._jsonService.SetFileContent(this._fileName, entities);
        }

        public bool Update(Guid uuid, T entity)
        {
            var entities = this._jsonService.GetFileContent<List<T>>(this._fileName);
            if (entities == null)
                return false;

            if (!entities.Any(ent => ent.Uuid == entity.Uuid))
                return false;

            var foundEntityIndex = entities.FindIndex(ent => ent.Uuid == entity.Uuid);
            entities[foundEntityIndex] = entity;

            return this._jsonService.SetFileContent(this._fileName, entities);

        }

        public bool Delete(T entity)
        {
            var entities = this._jsonService.GetFileContent<List<T>>(this._fileName);
            if (entities == null)
                return false;

            if (!entities.Any(ent => ent.Uuid == entity.Uuid))
                return false;

            entities = entities.Where(ent => ent.Uuid != entity.Uuid).ToList();
            return this._jsonService.SetFileContent(this._fileName, entities);
        }
    }
}
