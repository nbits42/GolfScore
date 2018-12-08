using System;
using GalaSoft.MvvmLight;
using GlobalContracts.Interfaces;
using Microsoft.WindowsAzure.MobileServices;

namespace TeeScore.Domain
{
    public class DomainBase: ObservableObject,  IEntityData
    {
        public static  DateTime EmptyDate = new DateTime(1900,1,1);

        public string Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }

        public bool IsNew => string.IsNullOrEmpty(Id);

        public DomainBase()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
