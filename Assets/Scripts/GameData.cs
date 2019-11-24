using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game/Create GameData")]
public class GameData : ScriptableObject
{
    public int Level;
    public int Coins;
    public bool NoAds;
    public string AvatarName;
}
