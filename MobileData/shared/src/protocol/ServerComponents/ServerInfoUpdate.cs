namespace shared
{
	/**
	 * Send from SERVER to all CLIENTS to provide info on how many people are in the lobby
	 * and how many of them are ready.
	 */
	public class ServerInfoUpdate : ASerializable
	{
		public int memberCount;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(memberCount);
		}

		public override void Deserialize(Packet pPacket)
		{
			memberCount = pPacket.ReadInt();
		}
	}
}
