using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Messaging
{
	public interface IMessageBus
	{
		void Subscribe<T>(IMessageHandler handler) where T : IMessage;
		void PublishMessage<T>(T message) where T : IMessage;
	}
}
