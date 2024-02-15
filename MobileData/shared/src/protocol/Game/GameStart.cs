namespace shared
{
    /**
     * Send from SERVER to CLIENT to indicate the game has started.
     * This includes the names of the players and the initial board state.
     */
    public class GameStart : ASerializable
    {
        public string player1;
        public string player2;
        public TicTacToeBoardData board = new TicTacToeBoardData();

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player1);
            pPacket.Write(player2);
            pPacket.Write(board);
        }

        public override void Deserialize(Packet pPacket)
        {
            player1= pPacket.ReadString();
            player2 = pPacket.ReadString();
            board = pPacket.Read<TicTacToeBoardData>();
        }
    }
}
