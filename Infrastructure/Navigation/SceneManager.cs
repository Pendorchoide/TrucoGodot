using Godot;

namespace TrucoProject.Infrastructure.Navigation {
    public partial class SceneManager : Node {

        private static SceneManager _instance;
        public static SceneManager GetInstance() => _instance;

        public override void _Ready() {
            _instance = this;
        }

        public void ChangeScene(string path) {
            CallDeferred(nameof(DeferredChangeScene), path);
        }

        public void ChangeScene(string path, string argument) {
            CallDeferred(nameof(DeferredChangeSceneWithArg), path, argument);
        }

        private void DeferredChangeScene(string path) {
            GetTree().ChangeSceneToFile(path);
        }

        private void DeferredChangeSceneWithArg(string path, string arg) {
            GetTree().ChangeSceneToFile(path);
            GetTree().CurrentScene.SetMeta("arg", arg);
        }

        public void Quit() => GetTree().Quit();
    }
}