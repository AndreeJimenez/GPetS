using GPetS.Extensions;
using GPetS.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPetS.Data
{
    public class PetsDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() => {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;

        static bool initialized = false;

        public PetsDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PetModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PetModel)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<PetModel>> GetAllPetsAsync()
        {
            return Database.Table<PetModel>().ToListAsync();
        }

        public Task<List<PetModel>> GetPetsNotDoneAsync()
        {
            return Database.QueryAsync<PetModel>($"SELECT * FROM [{typeof(PetModel).Name}] WHERE [Done] = 0");
        }

        public Task<PetModel> GetPetAsync(int id)
        {
            return Database.Table<PetModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SavePetAsync(PetModel item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeletePetAsync(PetModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}