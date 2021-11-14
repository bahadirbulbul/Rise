using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Settings
{
    internal class DBSettings : IDBSettings
    {
        public string PersonCollectionName { get; set; }
        public string PersonContactCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
