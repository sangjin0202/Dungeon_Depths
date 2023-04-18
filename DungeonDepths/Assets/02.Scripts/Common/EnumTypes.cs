namespace EnumTypes
{
    public enum MapType { KEY, NORMAL, BOSS, FINALBOSS, START }   //맵 종류 : 버튼마다 이미지가 달라야함
    public enum MapTheme { NONE, NATURE, DARK }                   //Stage 테마 : 이걸로 어느 스테이지의 맵인지 구분
    public enum MapDifficulty { NONE = 0, EASY, NORMAL, HARD }    //난이도 -> (int)형으로 받아와서 몬스터 스탯값에 곱해주거나 해야할듯 
    public enum MonsterID { Chomper, SPITTER, BEHOLDER, MIMIC}

    public enum Window { MAINMENU, GAMEOVER, OPTION, MAP}
}
