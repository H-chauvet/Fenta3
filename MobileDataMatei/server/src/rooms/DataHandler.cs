using shared;
using System;
using System.Collections.Generic;

namespace server
{
	/**
	 * This is the main server room where we'll be sending and receiving all the data
	 * The idea is that the phone will serialize and then send data to here
	 * * And the PC game will be asking for messages, which it will deserialize and do with depending on what's in the package 
	 */
	class DataHandler : SimpleRoom
	{
		

		public DataHandler(TCPGameServer pOwner) : base(pOwner)
		{
		}

		protected override void addMember(TcpMessageChannel pMember)
		{
			base.addMember(pMember);


			//print some info in the lobby (can be made more applicable to the current member that joined)
			ChatMessage simpleMessage = new ChatMessage();
			simpleMessage.message = _server.GetPlayerInfo(pMember).playerName + " has joined the lobby.";
			sendToAll(simpleMessage);

			//send information to all clients that the lobby count has changed
			sendServerUpdateCount();
		}

		protected override void removeMember(TcpMessageChannel pMember)
		{
			base.removeMember(pMember);

			sendServerUpdateCount();
		}

		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage, pSender);
			if (pMessage is GyroData) handleGyroData(pMessage as GyroData, pSender);
			if (pMessage is LightData) handleLightData(pMessage as LightData, pSender);
			if (pMessage is JoystickData) handleJoystickData(pMessage as JoystickData, pSender);
		}

		

		private void handleChatMessage(ChatMessage pChatMessage, TcpMessageChannel pSender)
		{
			//add the name of the sender to the message
			pChatMessage.message = _server.GetPlayerInfo(pSender).playerName + ": " + pChatMessage.message;

			//send the message to all clients in the lobby
			sendToAll(pChatMessage);
		}

		private void handleGyroData(GyroData pGyroData, TcpMessageChannel pSender)
		{
			sendToAll(pGyroData);
		}

		private void handleLightData(LightData pLightData, TcpMessageChannel pSender)
		{
			sendToAll(pLightData);
		}

		private void handleJoystickData(JoystickData pJoystickData, TcpMessageChannel pSender)
		{
			sendToAll(pJoystickData);
		}

		private void sendServerUpdateCount()
		{
			ServerInfoUpdate serverInfoMessage = new ServerInfoUpdate();
			serverInfoMessage.memberCount = memberCount;
			sendToAll(serverInfoMessage);
		}

	}
}
