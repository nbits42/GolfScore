using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public abstract class DomainBase: ObservableObject,  IEntityData
    {
        public string Id { get; set; }

        public bool IsNew => string.IsNullOrEmpty(Id);
    }
}
