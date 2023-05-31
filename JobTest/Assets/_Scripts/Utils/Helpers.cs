using System;

public class Helpers
{
    public struct AnimatorUpdateData
    {
        public bool IsRunning;
        public bool ReceivedHit;
        public bool InitiatedAttack;
        public bool Died;
    }

    public static eScreenType GetAppropriateScreenTypeByGameState()
    {
        switch (GameStateController.Instance.GameState)
        {
            case eGameState.MainMenu:
                return eScreenType.MainMenu;
            case eGameState.Fighting:
                return eScreenType.Fighting;
            case eGameState.GameOver:
                return eScreenType.GameOver;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
