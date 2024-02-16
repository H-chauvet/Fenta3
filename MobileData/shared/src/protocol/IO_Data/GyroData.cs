

namespace shared
{ 
    public class GyroData: ASerializable
    {
		public float gX;
		public float gY;
		public float gZ;
		public float gW;

		public override void Serialize(Packet pPacket)
		{
			pPacket.Write(gX.ToString());
			pPacket.Write(gY.ToString());
			pPacket.Write(gZ.ToString());
			pPacket.Write(gW.ToString());
		}

		public override void Deserialize(Packet pPacket)
		{
			string sX = pPacket.ReadString();
			gX = float.Parse(sX);

			string sY = pPacket.ReadString();
			gY = float.Parse(sY);

			string sZ = pPacket.ReadString();
			gZ = float.Parse(sZ);

			string sW = pPacket.ReadString();
			gW = float.Parse(sW);
		}
	}
}
