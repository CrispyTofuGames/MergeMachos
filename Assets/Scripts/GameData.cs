public static class GameData
{
    public static int[] characterFragmentCapsByLevel = new int[] { 50, 100, 150,0};
    public static int[] specialSkinsFragmentCapsByRarity = new int[] { 50, 100, 150, 200, 500 };
    public static int[] customiceFragmentsCapByRarity = new int[] {50,100,150,200};
    
}



public enum RewardType { SoftCoins, HardCoins, SpeedUpTime,SpeedUp, UnlockSpin, UnlockMissions, UnlockCustomization, Gifts, CharacterSkin, CharacterSpecialSkin, CharacterFragments, SkinFragments, CustomizeCellFragments, CustomizeExpositorFragments, CustomizeGroundFragments, CustomizationElement, CharacterLevelUp, LootBox};
public enum FragmentType {CharacterFragments, SkinFragments, CellFragments, ExpositorFragments, GroundFragments};
public enum CustomizeElementType {Cell, Expositor, Ground };