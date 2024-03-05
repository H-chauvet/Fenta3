

namespace shared
{
	public class LookData : ASerializable
	{
		public float lX;
		public float lY;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(lX.ToString());
			pPacket.Write(lY.ToString());
		}

		public override void Deserialize(Packet pPacket)
		{
			string sLX = pPacket.ReadString();
			lX = float.Parse(sLX);

			string sLY = pPacket.ReadString();
			lY = float.Parse(sLY);
		}
	}
}
