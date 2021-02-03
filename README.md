# Update Manager Package for Unity
Update management system made to give control on Unity's Update method
## How to use
### Basic Flow
	1. Implement at least one of the interfaces avalaible:
		- IUpdateable
		- IFixedUpdateable
		- ILateUpdateable
	2. Place your code on the method
		All the Update methods have a correspondent Interface.
	3. Subscribe to the UpdateManager by calling the static methods:
		- Subscribe
		- SubscribeFixed
		- SubscribeLate
	4. On runtime, the UpdateManager will map or create itself if there is no component in the scene.
		There is no need to create an object manually, but won't create any problems.
		The UpdateManager implements the Singleton Pattern.
### Unsubscribing
	1. Unsubscribe from the UpdateManager by calling the static methods:
		- UnSubscribe
		- UnSubscribeFixed
		- UnSubscribeLate
	2. If the object was subscribed, it will be removed from the updateable list,
		else there won't be any changes.
## Extra tools
### CountDown Timer
A simple timer that will raise an event when it's time has finished.
#### Examples
Simple/Internal cooldowns that don't need to update a view (for this, it might be better to use an ActionOverTime).
### Action Over Time
A helper that will call a method every Update cycle over a period of time.
Perfect for temporary behaviour, things you need to do over a period of time but have a start point and an end point.
#### It works with lerps
if you need to lerp in the action, you just need to set it in the constructor method to **true** and the method called will be given the lerp state as parameter.
#### Examples
Sliding doors when entering/exiting a trigger zone.
### Action With Frequency
A helper that will call a method every period of time.
The best solution for behaviour where you would normally implement a repeating timer or a coroutine.
#### Examples
Consuming/refilling stamina.
Recalculating pathfinding algorithms.
		
Hope you can take advantage of this package!
