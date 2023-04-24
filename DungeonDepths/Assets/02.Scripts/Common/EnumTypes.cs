namespace EnumTypes
{
    public enum MapType { KEY, NORMAL, BOSS, FINALBOSS, START }   //맵 종류 : 버튼마다 이미지가 달라야함
    public enum MapTheme { NONE, NATURE, DARK }                   //Stage 테마 : 이걸로 어느 스테이지의 맵인지 구분
    public enum MapDifficulty { NONE = 0, EASY, NORMAL, HARD }    //난이도 -> (int)형으로 받아와서 몬스터 스탯값에 곱해주거나 해야할듯 
    public enum MonsterID { Chomper, SPITTER, BEHOLDER, MIMIC }
    public enum Window { BOSSHPBAR, PLAYERSTATE, STAGETITLE, GETCARD, SELECTCARD, MAINMENU, GAMEOVER, MAP, OPTION, LOADING }
    public enum Option { FULLHD, CAMERASHAKE, SOUND, MOUSE, STATUS}
    public enum CardRarity { NOMAL, RARE }
    public enum Class { NONE, SWORD, GUN, MAGIC }
    public enum CardID
    {
        CARD_SPRINT,        // 이속 증가
        CARD_FRENZY,        // 공속 증가
        CARD_SNIPER,        // 공격 범위(사거리) 증가
        CARD_POWER,         // 공격력 증가
        CARD_JUMP,          // 점프 횟수 증가
        CARD_DEFENSE,       // 피격 데미지 감소
        CARD_COOLDOWN,      // 스킬 쿨타임 감소
        CARD_REBIRTH,       // 부활
        CARD_BARRIER,       // 보호막
        CARD_LIFESTEAL,     // 기본 공격시 피흡
        CARD_REGEN,         // 일정 시간동안 맞지않으면 체력 재생
        CARD_BERSERK,       // 체력이 낮아지면 이속 대폭 증가
        CARD_EXPLODE,       // 몹 처치시 폭발하며 광역 피해
        CARD_EXECUTE,       // 일정량 HP 이하의 잡몹 처형
        CARD_AMPLIFY,       // 크리티컬시 데미지 증폭
        CARD_BOSS,          // 보스 추뎀
        CARD_POISON,        // 기본 공격 독 속성
        CARD_BLOODLOSS,     // 기본 공격시 일정확률로 출혈
        CARD_FIRE,          // 기본 공격 불 속성
        CARD_ICE,           // 기본 공격 얼음 속성
        CARD_SHIELD,        // (전사)막기 성공시 적 스턴
        CARD_EARTHQUAKE,    // (전사)땅 찍기 사용시 스턴
        CARD_STING,         // (전사) 찌르기 히트박스 증가
        CARD_FLASH,         // (마법사) 순간이동 시 데미지
        CARD_METEOR,        // (마법사) 메테오 투사체 추가
        CARD_FROZENFIELD,   // (마법사) 얼음장판 넓이, 둔화 증가
        CARD_ROLL,          // (총잡이) 구르기 할때 섬광탄 투척
        CARD_FLASHBANG,     // (총잡이) 섬광탄 넓이 증가
        CARD_GENOCIDE,      // (총잡이) 난사시 이동, 범위, 지속시간 증가
        CARD_PET
    }
}
