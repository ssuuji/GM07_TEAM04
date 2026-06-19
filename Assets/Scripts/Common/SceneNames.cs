using System.Collections.Generic;

//씬 이름을 관리하는 정적 클래스
public class SceneNames
{
    private static readonly Dictionary<SceneType, string> sceneTable = new Dictionary<SceneType, string>()
    {
        {SceneType.Title, "TitleScene"},
        {SceneType.Game, "GameScene"}, 
        {SceneType.GameOver, "GameOverScene"}
    };

    public static string GetSceneName(SceneType sceneType)
    {
        return sceneTable[sceneType];
    }
}
