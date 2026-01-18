using Godot;

public partial class SceneManager : Node {
	SceneTree tree = (SceneTree)Engine.GetMainLoop();

	private string PrevScene { get; set; } = "";
	private string CurrentScene { get; set; } = "";
	private string MessagePreviousScene { get; set; } = "";

	private static SceneManager Instance;

	public static SceneManager GetInstance() {
		if (Instance == null) {
			Instance = new SceneManager();
		}

		return Instance;
	}

	public void ChangeScene(string path, string msg = null) {
		PrevScene = CurrentScene;
		CurrentScene = path;

		MessagePreviousScene = msg;

		tree.ChangeSceneToFile(path);
	}
}
