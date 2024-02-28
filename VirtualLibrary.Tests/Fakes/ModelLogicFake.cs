using VirtualLibrary.Logic.Interface;
using VirtualLibrary.Models;

namespace VirtualLibrary.Tests.Fakes
{
    public class ModelLogicFake<TEntity, TModel> : IModelLogic<TEntity, TModel> where TEntity : class
    {
        public IEnumerable<TEntity> _entities;
        public ModelLogicFake(IEnumerable<TEntity> entities)
        {
            _entities = entities;
        }
        
        private TEntity? CreateEntityFromDTO(TModel model)
        {
            return typeof(TEntity).Name switch
            {
                "Publisher" => new Publisher
                {
                    Name = (model as PublisherDTO)?.Name
                } as TEntity,
                "MagazineCopy" => new MagazineCopy
                {
                    IssureNumber = (model as MagazineDTO)?.IssueNumber ?? -1
                } as TEntity,
                "BookCopy" => new BookCopy
                {
                    Isbn = (model as BookDTO)?.Isbn ?? -1
                } as TEntity,
                "ArticleCopy" => new ArticleCopy
                {
                    Version = (model as ArticleDTO)?.Version ?? -1
                } as TEntity,
                _ => null
            };
        }

        private void UpdateEntityFromDTO(TEntity entity,TModel model)
        {
            object value = typeof(TEntity).Name switch
            {
                "Publisher" => (entity as Publisher).Name = (model as PublisherDTO).Name,
                "MagazineCopy" => (entity as MagazineCopy).IssureNumber = (model as MagazineDTO).IssueNumber,
                "BookCopy" => (entity as BookCopy).Isbn = (model as BookDTO).Isbn,
                "ArticleCopy" => (entity as ArticleCopy).Version = (model as ArticleDTO).Version,
            };
        }
        public async Task<Response> AddDataAsync(TModel entityDTO)
        {
            try
            {
                if(entityDTO == null)
                {
                    throw new Exception();
                }

                var newEntity = CreateEntityFromDTO(entityDTO) ?? throw new Exception();
                (_entities as ICollection<TEntity>).Add(newEntity);

                return new Response<TEntity>
                {
                    Success = true,
                    Data = newEntity,
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
        }

        public async Task<Response> DeleteDataAsync(string id)
        {
            try
            {
                var idProperty = typeof(TEntity).GetProperties().First(x => x.Name.ToLower().Contains(nameof(id)));
                var entityToDelete = _entities.First(x => idProperty.GetValue(x).Equals(int.Parse(id)));

                (_entities as ICollection<TEntity>).Remove(entityToDelete);
                return new Response<TEntity>
                {
                    Success = true,
                    Data =entityToDelete,
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
        }

        public async Task<Response> GetDataAsync()
        {
            try
            {
                return new Response<IEnumerable<TEntity>>
                {
                    Success = true,
                    Data = _entities.ToList(),
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
            
        }

        public async Task<Response> GetDatabyId(string id)
        {
            try
            {
                var idProperty = typeof(TEntity).GetProperties().First(x => x.Name.ToLower().Contains(nameof(id)));

                return new Response<TEntity>
                {
                    Success = true,
                    Data = _entities.First(x => idProperty.GetValue(x).Equals(int.Parse(id))),
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
        }

        public async Task<Response> GetSortedDataAsync(string modelField)
        {
            try
            {
                bool hasProperty = typeof(TEntity).GetProperties().Any(x => x.Name == modelField);
                if(!hasProperty)
                {
                    throw new Exception();
                }
                return new Response<IEnumerable<TEntity>>
                {
                    Success = true,
                    Data = _entities.ToList(),
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
        }

        public async Task<Response> UpdateDataAsync(string id, TModel entityDTO)
        {
            try
            {
                var idProperty = typeof(TEntity).GetProperties().First(x => x.Name.ToLower().Contains(nameof(id)));
                var entityToUpdate = _entities.First(x => idProperty.GetValue(x).Equals(int.Parse(id)));
                UpdateEntityFromDTO(entityToUpdate, entityDTO);

                return new Response<TEntity>
                {
                    Success = true,
                    Data = entityToUpdate,
                };
            }
            catch
            {
                return new Response
                {
                    Success = false,
                };
            }
        }
    }
}
