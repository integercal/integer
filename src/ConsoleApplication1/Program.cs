using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Agenda;
using Integer.Infrastructure.Repository;
using Raven.Client;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DocumentStoreHolder.Initialize();
            using (var session = DocumentStoreHolder.DocumentStore.OpenSession())
            {
                var eventos = session.Query<Evento>().ToList();

                foreach (var evento in eventos)
                {
                    if (evento.Id.Contains("/"))
                    {
                        var novoEvento = ObjectCopier.Clone<Evento>(evento);
                        novoEvento.Id = novoEvento.Id.Replace("/", "-");

                        session.Store(novoEvento);
                        session.Delete(evento);
                    }
                }

                session.SaveChanges();
            }
        }
    }

    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
