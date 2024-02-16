

namespace shared
{ 
    public class GyroData: ASerializable
    {
		public float iValue;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(iValue.ToString());
		}

		public override void Deserialize(Packet pPacket)
		{
			string sValue = pPacket.ReadString();
			iValue = float.Parse(sValue);
		}
	}
}
