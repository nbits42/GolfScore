using System;
using System.Collections.Generic;
using System.Text;

namespace TeeScore.Contracts
{
    public enum  CreateGameState
    {
        Idle,
        VenueSelection,
        VenueSelected,
        PropertiesSelection,
        PropertiesSelected,
        InvitationSelection,
        InvitiationCreated,
        InvitiationsAccepting,
        InvitiationsAccepted,
        Ready
    }
}
