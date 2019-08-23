I don't use Unity physics for Leto's movement.

It took me three years to realize that no matter what I did, I was never going to get that snappy, responsive movement that the games of yore had.

This code is a mess, but in short, I use Transform.Translate calls everywhere. Leto herself has 12-16 raycasts pointing in all directions to handle collision detection.

Making this public will probably make me clean up my code, which is never a bad thing. :)
