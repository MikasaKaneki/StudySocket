using System.Collections.Generic;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }

    public class Room
    {
        private List<Client> _clientRoom = new List<Client>();
        private RoomState _state = RoomState.WaitingJoin;



    }
}