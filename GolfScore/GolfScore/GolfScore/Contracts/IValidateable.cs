using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TeeScore.Contracts
{
    public interface IValidateable: IDataErrorInfo
    {
        ObservableCollection<Tuple<string, string>> InvalidProperties
        { get; }
        bool IsValid { get; }
    }
}
