using System;
using System.Collections.Generic;
using System.Text;
using GlobalContracts.Interfaces;

namespace GolfScore.Domain
{
    public abstract class DomainBase: IEntityData
    {
        public string Id { get; set; }
    }
}
