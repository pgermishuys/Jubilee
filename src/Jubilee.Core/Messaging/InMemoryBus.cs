using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jubilee.Core.Messaging
{
	public class InMemoryBus : IMessageBus
	{
		private readonly Dictionary<Type, List<IMessageHandler>> handlerLookup = new Dictionary<Type, List<IMessageHandler>>();
		public InMemoryBus()
		{
		}

		public void PublishMessage<T>(T message) where T : IMessage
		{
			DispatchByType(message);
		}

		public void Subscribe<T>(IMessageHandler handler) where T : IMessage
		{
			List<IMessageHandler> handlers;
			var type = typeof(T);
			if (!handlerLookup.TryGetValue(type, out handlers))
			{
				handlerLookup.Add(type, handlers = new List<IMessageHandler>());
			}
			if (!handlers.Any(x => x.Equals(handler)))
			{
				handlers.Add(handler);
			}
		}

		private void DispatchByType(IMessage message)
		{
			var type = message.GetType();
			do
			{
				DispatchByType(message, type);
				type = type.BaseType;
			} while (type != null && type != typeof(IMessage));
		}

		private void DispatchByType(IMessage message, Type type)
		{
			List<IMessageHandler> list;
			if (!handlerLookup.TryGetValue(type, out list)) return;
			foreach (var handler in list)
			{
				Task.Factory.StartNew(() => { handler.Handle(message); });
			}
		}
	}
}
