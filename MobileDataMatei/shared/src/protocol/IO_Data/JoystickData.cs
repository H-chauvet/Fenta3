

namespace shared
{
	public class JoystickData : ASerializable
	{
		public float jX;
		public float jY;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(jX.ToString());
			pPacket.Write(jY.ToString());
		}

		public override void Deserialize(Packet pPacket)
		{
			string sJX = pPacket.ReadString();
			jX = float.Parse(sJX);

			string sJY = pPacket.ReadString();
			jY = float.Parse(sJY);
		}
	}
}
