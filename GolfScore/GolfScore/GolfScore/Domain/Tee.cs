using System;
using System.Collections.Generic;
using System.Text;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class Tee: DomainBase, ITee
    {
        public DateTime Finished { get; set; } = DomainBase.EmptyDate;
        public string GameId { get; set; }
        public string Number { get; set; }
        public DateTime Started { get; set; } = DomainBase.EmptyDate;
    }
}
