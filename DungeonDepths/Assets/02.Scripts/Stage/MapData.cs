using UnityEngine;
using EnumTypes;

[CreateAssetMenu(fileName = "Map Data", menuName = "Scriptable Object/Map Data", order = int.MaxValue)]
public class MapData : ScriptableObject
{
    [SerializeField]
    private string    mapName;          //¸Ê ÀÌ¸§
    [SerializeField]
    private Vector3   position;         //¸Ê À§Ä¡
    [SerializeField]
    private MapType   type;             //¸Ê Å¸ÀÔ
    [SerializeField]
    private MapTheme  theme;            //ÇöÀç ¸Ê Å×¸¶
    [SerializeField]
    private MapDifficulty difficulty;   //³­ÀÌµµ
    [SerializeField]
    private int totalBoxNum;            //¸Ê¿¡ Á¸ÀçÇÏ´Â ÃÑ º¸¹° »óÀÚ ¼ö
    [SerializeField]
    private int totalMonsterNum;        //¸Ê¿¡ ¼ÒÈ¯µÇ´Â ÃÑ ¸ó½ºÅÍ ¼ö

    public string MapName
    {
        get => mapName;
        set => mapName = value;
    }
    public Vector3 Position
    {
        get => position;
        set => position = value;
    }
    public MapType Type
    {
        get => type; 
        set => type = value;    
    }
    public MapTheme Theme
    {
        get => theme;
        set => theme = value;
    }
    public MapDifficulty Difficulty
    {
        get => difficulty;
        set => difficulty = value;
    }
    public int TotalBoxNum
    {
        get => totalBoxNum;
        set => totalBoxNum = value;
    }
    public int TotalMonsterNum
    {
        get => totalMonsterNum;
        set => totalMonsterNum = value;
    }
}
