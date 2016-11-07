namespace KKings.Foundation.Popsicle.xDb.Ingest.DataReader
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;

    public class DefaultDataReader : IDataReader
    {
        public class DefaultContact
        {
            public string FirstName { get; set; }

            public string Surname { get; set; }

            public string Title { get; set; }

            public string JobTitle { get; set; }

            public string Gender { get; set; }
        }

        public Stream GetDataStream()
        {
            var @default = new DefaultContact
            {
                FirstName = "My First Name",
                Surname = "My Last Name",
                Title = "Lord",
                Gender = "Male",
                JobTitle = "Apprentice"
            };

            var serialized = JsonConvert.SerializeObject(@default);

            return new MemoryStream(Encoding.UTF8.GetBytes(serialized));
        }
    }
}