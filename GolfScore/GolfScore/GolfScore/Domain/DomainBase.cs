using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public abstract class DomainBase: IEntityData
    {
        public string Id { get; set; }
    }
}
