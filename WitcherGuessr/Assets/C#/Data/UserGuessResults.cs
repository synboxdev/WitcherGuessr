using System;
using System.Collections.Generic;

[Serializable]
public class UserGuessResults
{
    public int LocationNumber;
    public int AvailableAttempts;
    public List<UserGuessAttempt> UserGuesses = new List<UserGuessAttempt>();

    public UserGuessResults()
    {
        LocationNumber = 1;
        AvailableAttempts = 3;
    }
}