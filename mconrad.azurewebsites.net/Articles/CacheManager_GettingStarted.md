## Getting started with Cache Manager

### Basics
If you download one of the Cache Manager's nuget packages you'll find
that there is a CacheManager.Core package. This package contains 
the base implementation of the Cache Manager.

The [Core][1] Namespace contains the main interfaces and classes you will configure the cache application and work with.

All other packages, like the [CacheManager.SystemRuntimeCaching][2] primarily contain a cache implementation for 
the main cache manager based on a specific cache provider (in this case System.Runtime.Caching).

### Architecture
First of all, Cache Manager will provide well known cache methods like Get, Put, Remove... But the 
main architectural feature is that the Cache Manager can handle multiple different cache layers, the so called 
cache handles.  
The Manager can have many handles, and we have one cache handle for each supported cache provider.

This makes it very flexible in terms of caching strategy! And it makes it easy to start with in the first place, because 
you might not know how complex a cache must be at the start of the project. Or you just want to test something out first and later 
you realize that you need a more complex scenario.

#### Example Usecase
A common, more complex scenario would be, that you have a distributed cache, e.g. Redis, and you want to access that layer 
from multiple clients and share the cached data across those clients because the creation of the cached items is expensive, or 
you want to store data in one place and use it by multiple clients...  

Distributed caches are fast, but not as fast as in-process caches which keep your cache values in memory and do not have 
to use expensive serialization or network resources.   

In addition to the performance differernce, usually an application will read from cache a lot more than writing to it. 
Now if we put an in-process cache in front of the distributed cache, to read directly from memory, this would drastically increase the overall 
application's performance.   
To give you just a rough idea of the read performance difference, it can be up to 100 times faster or even more...
If a Redis cache for example can handle 10k Gets per second, a memory cache can perfrom 2 million.

#### Challenges when mixing distributed and in-process cache
There are some challenges with this scenario. We now store cache values in memory, what happens if the cache item was removed from cache by one client...
Of course, it will still be cached in memory by all other clients.

Also updating a cache item in a distributed cache is different. With in process caches, we can ensure thread safe writes, with 
distributed caches, we cannot do it that easily. Every distributed cache provider has some slightly different ways to handle that...

**The good new is, that CacheManager handles all that for you behind the scenes!**

### Cache Manager
Now how does the BaseCacheManager handle items in mulitple caches?  
This depends on configuration in some cases, lets have a look at the basic cache operations:

#### Set and Put
Set and Put adds and/or overrides a cached value. The cache manager will add or put the cache item 
into all configured cache handles. This is necessairy because in general we want to have all 
layers of our cache in sync.

#### Get operations




[1]:(http://michaco.github.io/Documentation/CacheManager/Help/html/N_CacheManager_Core.htm)
[2]:(https://www.nuget.org/packages/CacheManager.SystemRuntimeCaching/)