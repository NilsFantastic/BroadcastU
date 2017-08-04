using System;
using AlmostFantastic.Messaging;

namespace Example
{
    public class ExampleReceiveEventInNamedFunction: ExampleBase
    {
        public ExampleReceiveEventInNamedFunction()
        {
            BindEvent();
            BroadcastEvent();
            
        }
        void BroadcastEvent()
        {
            var exampleCustomEventData = typeof(ExampleEvent).Name + " received in class " + GetType().Name;
            // Publish the event "ExampleEvent"
            BroadcastU.Instance.Publish(new ExampleEvent{ExampleParameter = exampleCustomEventData});
        }

        void BindEvent()
        {
            //Set up an event Listener
            BroadcastU.Instance.Subscribe<ExampleEvent>(ExampleEventHandler);
        }

        void ExampleEventHandler(object evtobj)
        {
            // Cast the event object to Example event to get access to the event's content if needed
            ExampleEvent evt = evtobj as ExampleEvent;
            Console.WriteLine(evt.ExampleParameter);
        }
    }
}