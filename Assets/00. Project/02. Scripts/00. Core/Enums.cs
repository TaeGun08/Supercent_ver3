public enum SoundType
{
    None = 0,
    MiningHit_Burst,        // 단발성 채굴 (곡괭이 등)
    MiningHit_Continuous,   // 연속성 채굴 (드릴, 불도저)
    ItemPickup_Default,     // 일반 아이템 획득 (조약돌, 포션)
    ItemDrop_Default,       // 일반 아이템 하적
    ItemPickup_Gold,        // 골드 획득
    ItemDrop_Gold,          // 골드 투입/하적
    UnlockSuccess,
    PrisonerEnter,
    ButtonClick
}
