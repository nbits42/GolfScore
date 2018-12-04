using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;

namespace TeeScore.Domain
{
    public class DomainBase: ObservableObject,  IEntityData
    {
        public static  DateTime EmptyDate = new DateTime(1900,1,1);
        public string Id { get; set; }

        public bool IsNew => string.IsNullOrEmpty(Id);
    }
}
