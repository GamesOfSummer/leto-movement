I don't use Unity physics for Leto's movement.

It took me three years to realize that no matter what I did, I was never going to get that snappy, responsive movement that the games of yore had.

This code is rather verbose, but in short, I use Transform.Translate calls everywhere. Leto herself has 12-16 raycasts pointing in all directions to handle collision detection. She uses the MonsterLove state machine pattern.

Where to start if you want to know how I move Leto -

    private void MoveHorizontal(float speed)
    {
        speed = Useful.instance.ConvertUnityUnitsToPixels(speed);
        var distanceToMove = _playerScript.CalculateHorizontalDistance(speed);

        GetComponent<Transform>().Translate(new Vector3(distanceToMove, 0, 0));
        GetComponent<Transform>().position = Useful.instance.RoundVectorToTwoPlaces(GetComponent<Transform>().position);
        GetComponent<Transform>().position = _playerScript.FinalAdjustHorizontalSpeed(speed);
    }

Making this public will probably make me clean up my code, which is never a bad thing. :)
