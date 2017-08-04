using System;
using System.Collections.Generic;
using AlmostFantastic.Messaging.Event;
namespace AlmostFantastic.Messaging
{
	public class BroadcastU
	{

		static volatile BroadcastU instance;
		static object syncRoot = new Object();
		Dictionary<Type, IList<Action<object>>> RunOnceSubscribers = new Dictionary<Type, IList<Action<object>>>();
		Dictionary<Type, IList<Action<object>>> Subscribers = new Dictionary<Type, IList<Action<object>>>();

		BroadcastU() {}
		
		public static BroadcastU Instance
		{
			get 
			{
				if (instance == null) 
				{
					lock (syncRoot) 
					{
						if (instance == null) 
							instance = new BroadcastU();
					}
				}			
				return instance;
			}
		}

		public void RemoveAllSubscribers()
		{
			var subscribers = Subscribers;
			var runOnceSubscribers = RunOnceSubscribers;
		 	Subscribers = new Dictionary<Type, IList<Action<object>>>();
			RunOnceSubscribers = new Dictionary<Type, IList<Action<object>>>();
			Publish<RemoveAllSubscribers>(new RemoveAllSubscribers(), subscribers);
			Publish<RemoveAllSubscribers>(new RemoveAllSubscribers(), runOnceSubscribers);
		}

	    public void Subscribe<TBroadcastMessageType>(Action handler)
	    {
	        Subscribe<TBroadcastMessageType>((obj)=>handler());
	    }

		public void Subscribe<TBroadcastMessageType>(Action<object> handler)
		{
			SubscribeOnSubscriberList<TBroadcastMessageType>(handler, ref Subscribers);
		}

	    public void SubscribeOnce<TBroadcastMessageType>(Action handler)
	    {
	        SubscribeOnce<TBroadcastMessageType>((obj) => handler());
	    }
		public void SubscribeOnce<TBroadcastMessageType>(Action<object> handler)
		{
			SubscribeOnSubscriberList<TBroadcastMessageType>(handler, ref RunOnceSubscribers);
		}

		void SubscribeOnSubscriberList<TBroadcastMessageType>(Action<object> handler, ref Dictionary<Type, IList<Action<object>>> subscriberDictionary)
		{
			IList<Action<object>> subscriberlist;
			if (subscriberDictionary.TryGetValue (typeof(TBroadcastMessageType), out subscriberlist)) 
				subscriberlist.Add (handler);
			else
				subscriberDictionary.Add (typeof(TBroadcastMessageType), new List<Action<object>>{ handler });
		}

		void Publish<TMessageType>(TMessageType e, Dictionary<Type, IList<Action<object>>> subscribers){
			IList<Action<object>> handlers;
			if(!subscribers.TryGetValue(typeof(TMessageType), out handlers))
				return;
			foreach (var handler in handlers) {
				handler(e);
			}
		}

		public void Publish<TMessageType>(TMessageType e){
			Publish(e, RunOnceSubscribers);
			Publish(e, Subscribers);
			RemoveAllSubscribersOfMessageType<TMessageType> (ref RunOnceSubscribers);
		}

	    public void RemoveHandler(Action<object> handler)
	    {
	        RemoveHandlerFromSubscriberList(ref RunOnceSubscribers, handler);
	        RemoveHandlerFromSubscriberList(ref Subscribers, handler);
	    }


	    public void RemoveAllSubscribersOfMessageType<TMessageType>()
	    {
	        RemoveAllSubscribersOfMessageType<TMessageType>(ref RunOnceSubscribers);
	        RemoveAllSubscribersOfMessageType<TMessageType>(ref Subscribers);
	    }

	    void RemoveHandlerFromSubscriberList(ref Dictionary<Type, IList<Action<object>>> subscriberdictionary, Action<object> handler )
	    {
	        foreach (var subscriber in subscriberdictionary)
	        {
	            foreach (var action in subscriber.Value)
	            {
	                if (handler != action) continue;
	                subscriber.Value.Remove(action);
	                break;
	            }
	        }
	    }
		void RemoveAllSubscribersOfMessageType<TMessageType>  (ref Dictionary<Type, IList<Action<object>>> subscriberDictionary)
		{
			var messageType = typeof(TMessageType);
			if(subscriberDictionary.ContainsKey(messageType))
			   subscriberDictionary.Remove(messageType);
		}
	}
}