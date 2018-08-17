using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIwithJWT.Models
{
    public class Accounts
    {
        private DiamondStoriesContext context;

        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Sessionid { get; set; }

        public string Sessionip { get; set; }
    }

    public class Launcher_Mods
    {
        private DiamondStoriesContext context;

        public int Id { get; set; }

        public string Modname { get; set; }

        public string Modurl { get; set; }

        public string Installpath { get; set; }

        public string Md5sum { get; set; }
    }
}
