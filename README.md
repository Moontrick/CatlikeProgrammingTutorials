CatlikeProgrammingTutorials
===========================
Bunch of tutorials that I'm working my way through that you can find here: http://catlikecoding.com/unity/tutorials/.  The difficulty ranges from very easy all the way up to pretty complex topics.


What I've done so far and what I learned from it:
------------------------------------------------

Clock: Pretty self explanitory.  It's a clock and it can function both in analog and digital modes.

Fractals: I've been drawn to fractals for a while but I've never made one in Unity before.  Deriving beautiful shapes from simpler derivites is fascinating to me.  One big thing I learned in the course of doing this tutorial is that when you change the color of a material, Unity silently creates a copy in the background.  This breaks dynamic batching since in order for that to work the same material must be used.  Good to know.

Maze: I like his use of what is basically a custom data type, IntVector2.  Unity's Vector2 only takes floats, so this comes in handy.  Using that in tandem with the MazeDirection class makes the entire thing a lot simpler than a lot of first-try implemenations might be.  
