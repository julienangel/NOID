# Somewhat Off-Kilter Rideshare

This project is a sample application for use as a code test for Toca Boca. 

__TO BE CLEAR__: This is not an example of a well-built project, or indicative of how games are built at Toca Boca.

Its purpose is to provide a functional, if somewhat dysfunctional, codebase for candidates to start with rather than having to build an entire game from scratch. 

# How did I approach the problem #
After I took a look on the game, I noticed that to implement a replay system(first time doing so), I figured that we needed to have some kind of timer.
And I believe with a timer the game would become more challenging to grab all the passengers and so on.
With that timer, became easier to allocate enough space(I'm counting to not set the timer to 10 months for example eheh) to save the state of the game.
Also, I found it easier to set some basic UI, where when the game ends, we can restart the game, or replay the last game played. And we can also end the game earlier with the End button.

# Happy path to utilize this solution #
To play the game, and watch the replay is fairly simple.
To utilize the replay, we have the replay manager where we can grab the replay, but only for the last game.

# How did I arrive to my solution #
As I mentioned, I implemented a solution with timer. With that, solved the problem with infinite replay, or very very large replays.
Also with time, we can pre allocate enough space to save the state of the car and the passengers.
All static mesh, I marked as static.
I implemented also some utils to help us in performance.

- Update manager(monobehaviours only update when they have to update)
- Dependency injection
- SO Architecture
- SO Game events and variables
- 0 usages of coroutines and only async/task methods
- cached transforms(transform calls, make usage of the extern(there is a cost))

I tried to make it the most flexible way, without hard references.
Also, when picking up a passenger, it was destroy instead of just deactivating.
For the replay manager, at the moment it is saving a very simple structure. Where basically saves the transform of the car, and in which frame a passenger deactivates.
I didn't want to overcomplicate it.

# Any hurdles #
I didn't see any. It was quite straightforward the test. I enjoyed it.

# Next steps #
Of course, this always depends on what the team/company would want.
But as a programmer, I would reduce a bit more of the hard references. 
The prefab of the car, should be injected. With that we would have more control, instead of hard referencing from the scene.
Would work better on the UI, more options as well.
And certainly I would work a bit more on the replay manager. To reduce the size of the saving. And maybe using multithreading to save.
And maybe also(would depend of the size), I would use the a NativeList, or a NativeQueue, instead of a List<T>.
