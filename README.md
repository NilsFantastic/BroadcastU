# BroadcastU
Stupid simple Pub-Sub broker / Broadcaster / Event manager for C# .net compatible with Unity3D (and all other c#) projects

The project contians example .cs-files 

## Good practices

- Good for letting multiple classes know of a change in the application that they need to react to
- Use for lazy invoke
- Do use for managing effects and to aid the storing of state
- Do not use to control the flow of the game/application as Events can never be exclusive to each other


Example

Event Class
```
public class ExampleEvent
{
    public string ExampleParameter;
}
```
In the broadcasting class:
```
void Start(){
      BroadcastU.Instance.Publish(new ExampleEvent{ExampleParameter = exampleCustomEventData});
}
```
In receiving class
```
void Start() {
  BroadcastU.Instance.Subscribe<ExampleEvent>(ExampleEventHandler);
}
void ExampleEventHandler(object evtobj)
{
     // Cast the event object to Example event to get access to the event's content if needed
     ExampleEvent evt = evtobj as ExampleEvent;
     Console.WriteLine(evt.ExampleParameter);
}
```
To remove an event handler
```
BroadcastU.Instance.RemoveHandler(ExampleEventHandler);
```

Or to remove all event handlers at once
```
BroadcastU.Instance.RemoveAllSubscribers();
```
