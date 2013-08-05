using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Integer.Infrastructure.Repository.Indexes;

namespace Integer.UnitTests
{
    public abstract class InMemoryDataBaseTest : IDisposable
    {
        IDocumentStore dataBase;
        protected IDocumentSession DataBaseSession { get; private set; }

        public InMemoryDataBaseTest() 
        {
            CriarNovoBancoDeDados();
            DataBaseSession = dataBase.OpenSession();
        }

        public void Dispose() 
        {
            DataBaseSession.Dispose();
            DataBaseSession = null;

            dataBase.Dispose();
        }

        protected void CriarNovoBancoDeDados() 
        {
            dataBase = new EmbeddableDocumentStore()
            {
                RunInMemory = true,

            };
            dataBase.Conventions.SaveEnumsAsIntegers = true;
            dataBase.Conventions.IdentityPartsSeparator = "-";
            var generator = new MultiTypeHiLoKeyGenerator(10);
            dataBase.Conventions.DocumentKeyGenerator = (dbname, commands, entity) => generator.GenerateDocumentKey(commands, dataBase.Conventions, entity);
            dataBase.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            dataBase.Initialize();

            IndexCreation.CreateIndexes(typeof(ReservasMap).Assembly, dataBase);
        }
    }
}
