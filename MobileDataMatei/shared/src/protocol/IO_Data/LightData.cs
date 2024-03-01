

namespace shared
{
	public class LightData : ASerializable
	{
		public float luxValue;
		

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(luxValue.ToString());
			
		}

		public override void Deserialize(Packet pPacket)
		{
			string sLux = pPacket.ReadString();
			luxValue = float.Parse(sLux);

			
		}
	}
}
