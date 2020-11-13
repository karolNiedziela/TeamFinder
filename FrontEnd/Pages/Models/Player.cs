using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace FrontEnd.Pages.Models
{
    public class Player : PlayerDTO
    {
        [DisplayName("Username")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [DisplayName("Age")]
        public override int Age { get => base.Age; set => base.Age = value; }

        [DisplayName("Info")]
        public override string Info { get => base.Info; set => base.Info = value; }
    }
}
