[System.Serializable]
public class UserData
{
    public string username;
    public string email;
    public int highscore;

    public UserData(string username, string email, int highscore)
    {
        this.username = username;
        this.email = email;
        this.highscore = highscore;
    }
}